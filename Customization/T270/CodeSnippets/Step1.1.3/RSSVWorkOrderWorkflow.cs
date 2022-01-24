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
            var context = config.GetScreenConfigurationContext<RSSVWorkOrderEntry,
                RSSVWorkOrder>();
            context.AddScreenConfigurationFor(screen =>
                screen
                .StateIdentifierIs<RSSVWorkOrder.status>()
                    .AddDefaultFlow(flow => ...));
        }
    }
}