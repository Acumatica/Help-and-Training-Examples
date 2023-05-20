using System;
using System.Collections.Generic;
using System.Linq;
using PX.BusinessProcess.Subscribers.ActionHandlers;
using PX.BusinessProcess.Subscribers.Factories;
using PX.BusinessProcess.Event;
using PX.BusinessProcess.DAC;
using PX.BusinessProcess.UI;
using System.Threading;
using PX.Data;
using PX.Common;
using PX.SM;
using System.IO;
using PX.Data.Wiki.Parser;
using PX.PushNotifications;

namespace CustomSubscriber
{
    //The custom subscriber that the system executes once the business event has occurred
    public class CustomEventAction : IEventAction
    {
        //The GUID that identifies a subscriber
        public Guid Id { get; set; }

        //The name of the subscriber of the custom type
        public string Name { get; protected set; }

        //The notification template
        private readonly Notification _notificationTemplate;

        //The method that writes the body of the notification to a text file once the business event has occurred
        public void Process(MatchedRow[] eventRows, CancellationToken cancellation)
        {
            using (StreamWriter file = new StreamWriter(@"C:\tmp\EventRows.txt"))
            {
                var graph = PXGenericInqGrph.CreateInstance(_notificationTemplate.ScreenID);
                var parameters = @eventRows.Select(r => Tuple.Create<IDictionary<string, object>, IDictionary<string, object>>(
                r.NewRow?.ToDictionary(c => c.Key.FieldName, c => c.Value),
                r.OldRow?.ToDictionary(c => c.Key.FieldName, c => (c.Value as ValueWithInternal)?.ExternalValue ?? c.Value))).ToArray();
                var body = PXTemplateContentParser.ScriptInstance.Process(_notificationTemplate.Body, parameters, graph, null);
                file.WriteLine(body);
            }
        }

        //The CustomEventAction constructor
        public CustomEventAction(Guid id, Notification notification)
        {
            Id = id;
            Name = notification.Name;
            _notificationTemplate = notification;
        }
    }

    //The class that creates and executes the custom subscriber
    class CustomSubscriberHandlerFactory : IBPSubscriberActionHandlerFactoryWithCreateAction
    {
        //The method that creates a subscriber with the specified ID
        public IEventAction CreateActionHandler(Guid handlerId, bool stopOnError, IEventDefinitionsProvider eventDefinitionsProvider)
        {
            var graph = PXGraph.CreateInstance<PXGraph>();
            Notification notification = PXSelect<Notification, Where<Notification.noteID, Equal<Required<Notification.noteID>>>>
                .Select(graph, handlerId).AsEnumerable().SingleOrDefault();

            return new CustomEventAction(handlerId, notification);
        }

        //The method that retrieves the list of subscribers of the custom type
        public IEnumerable<BPHandler> GetHandlers(PXGraph graph)
        {
            return PXSelect<Notification, Where<Notification.screenID, Equal<Current<BPEvent.screenID>>, Or<Current<BPEvent.screenID>, IsNull>>>
                .Select(graph).FirstTableItems.Where(c => c != null)
                .Select(c => new BPHandler { Id = c.NoteID, Name = c.Name, Type = LocalizableMessages.CustomNotification });
        }

        //The method that redirects to the subscriber
        public void RedirectToHandler(Guid? handlerId)
        {
            var notificationMaint = PXGraph.CreateInstance<SMNotificationMaint>();
            notificationMaint.Message.Current = notificationMaint.Notifications.Search<Notification.noteID>(handlerId);
            PXRedirectHelper.TryRedirect(notificationMaint, PXRedirectHelper.WindowMode.New);
        }

        //A string identifier of the subscriber type that is exactly four characters long
        public string Type
        {
            get { return "CTTP"; }
        }

        //A string label of the subscriber type
        public string TypeName
        {
            get { return LocalizableMessages.CustomNotification; }
        }

        //A string identifier of the action that creates a subscriber of the custom type
        public string CreateActionName
        {
            get { return "NewCustomNotification"; }
        }

        //A string label of the button that creates a subscriber of the custom type
        public string CreateActionLabel
        {
            get { return LocalizableMessages.CreateCustomNotification; }
        }

        //The delegate for the action that creates a subscriber of the custom type
        public Tuple<PXButtonDelegate, PXEventSubscriberAttribute[]> getCreateActionDelegate(BusinessProcessEventMaint maintGraph)
        {
            PXButtonDelegate handler = (PXAdapter adapter) =>
            {
                if (maintGraph.Events?.Current?.ScreenID == null)
                    return adapter.Get();

                var graph = PXGraph.CreateInstance<SMNotificationMaint>();
                var cache = graph.Caches<Notification>();
                var notification = (Notification)cache.CreateInstance();
                var row = cache.InitNewRow(notification);
                row.ScreenID = maintGraph.Events.Current.ScreenID;
                cache.Insert(row);

                var subscriber = new BPEventSubscriber();
                var subscriberRow = maintGraph.Subscribers.Cache.InitNewRow(subscriber);
                subscriberRow.Type = Type;
                subscriberRow.HandlerID = row.NoteID;
                graph.Caches[typeof(BPEventSubscriber)].Insert(subscriberRow);

                PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.NewWindow);
                return adapter.Get();
            };
            return Tuple.Create(handler,
                new PXEventSubscriberAttribute[]
                    {new PXButtonAttribute {OnClosingPopup = PXSpecialButtonType.Refresh}});
        }
    }

    //Localizable messages
    [PXLocalizable]
    public static class LocalizableMessages
    {
        public const string CustomNotification = "Custom Notification";
        public const string CreateCustomNotification = "Custom Notification";
    }
}
