using PX.Data;
using PX.Data.WorkflowAPI;
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
                        .WithFlowStates(...)
                        .WithTransitions(transitions =>
                        {
                            transitions.AddGroupFrom<States.onHold>(ts =>
                            {
                                ts.Add(t => t.To<States.readyForAssignment>()
                                    .IsTriggeredOn(g => g.ReleaseFromHold)
                                    .When(conditions.DoesNotRequirePrepayment)
                                    );
                                ts.Add(t => t.To<States.pendingPayment>()
                                    .IsTriggeredOn(g => g.ReleaseFromHold)
                                    .When(conditions.RequiresPrepayment));
                            });
                        })
                     )
                     .WithActions(...));
        }
    }
}