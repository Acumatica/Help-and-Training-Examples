using PX.Data;
using PX.Data.WorkflowAPI;
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
                            fss.Add<States.paid>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state =>
                                            state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state =>
                                            state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state =>
                                            state.IsDisabled());
                                    });
                            });
                        })
                        .WithTransitions(...)
                    )
                    .WithActions(...)
                    .WithCategories(c...)
                    .WithForms(...)
                    );
        }
    }
}