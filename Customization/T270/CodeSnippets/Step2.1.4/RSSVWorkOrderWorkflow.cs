using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Data.BQL.Fluent;
using PX.Objects.Common;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry,
  PhoneRepairShop.RSSVWorkOrder>;

namespace PhoneRepairShop.Workflows
{
    public class RSSVWorkOrderWorkflow :
      PX.Data.PXGraphExtension<RSSVWorkOrderEntry>
    {
        ...

        public override void Configure(PXScreenConfiguration config)
        {
            ...

            context.AddScreenConfigurationFor(screen =>
                screen
                    .StateIdentifierIs<RSSVWorkOrder.status>()
                    .AddDefaultFlow(flow =>
                        flow
                        .WithFlowStates(fss =>
                        {
                            ...
                            fss.Add<States.completed>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(...)
                                    .WithActions(...)
                                    .WithEventHandlers(handlers =>
                                    {
                                        handlers.Add(g => g.OnCloseDocument);
                                    });
                            });
                            ...
                        })
                        .WithTransitions(transitions =>
                        {
                            ...
                            transitions.AddGroupFrom<States.completed>(ts =>
                            {
                                ts.Add(t => t.To<States.paid>().IsTriggeredOn(g => g.OnCloseDocument));
                            });
                        })
                    )
                    .WithHandlers(handlers =>
                    {
                        handlers.Add(handler => handler
                            .WithTargetOf<ARInvoice>()
                            .OfEntityEvent<ARInvoice.Events>(e => e.CloseDocument)
                            .Is(g => g.OnCloseDocument)
                            .UsesPrimaryEntityGetter<
                            SelectFrom<RSSVWorkOrder>.
                            Where<RSSVWorkOrder.invoiceNbr.IsEqual<ARRegister.refNbr.FromCurrent>>>());
                    })
                    .WithActions(...)
                    .WithCategories(...)
                    .WithForms(...)
                    );
        }
    }
}