using PX.Data;
using PX.PushNotifications.UI.DAC;
using System;
using PX.Data.BQL.Fluent;
using PX.PushNotifications.Sources;

public class TestInCodeDefinition : IInCodeNotificationDefinition
{
    public Tuple<BqlCommand, PXDataValue[]> GetSourceSelect()
    {
        return
          Tuple.Create(
            SelectFrom<PushNotificationsHook>.
            LeftJoin<PushNotificationsSource>.
              On<PushNotificationsHook.hookId.
                IsEqual<PushNotificationsSource.hookId>>.View
            .GetCommand(), new PXDataValue[0]);
    }

    public Type[] GetRestrictedFields()
    {
        return new[]
        {
          typeof(PushNotificationsHook.address),
          typeof(PushNotificationsHook.type),
          typeof(PushNotificationsSource.designID),
          typeof(PushNotificationsSource.inCodeClass),
          typeof(PushNotificationsSource.lineNbr)
        };
    }
}
