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
                    .AddDefaultFlow(flow => flow
                        .WithFlowStates(...)
                        .WithTransitions(transitions =>
                        {
                            transitions.Add(t => t.From<States.onHold>()
                              .To<States.readyForAssignment>()
                              .IsTriggeredOn(g => g.ReleaseFromHold));
                        }))
                    .WithCategories(categories =>
                    {
                        categories.Add(processingCategory);
                    })
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.ReleaseFromHold, c => c
                                .WithCategory(processingCategory));
                    }));
        }
    }
}