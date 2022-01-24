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

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition RequiresPrepayment => GetOrCreate(b => b.FromBql<
              Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>,
              Equal<True>>>());

            public Condition DoesNotRequirePrepayment => GetOrCreate(b => b.FromBql<
              Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>,
              Equal<False>>>());
        }
        #endregion

        public override void Configure(PXScreenConfiguration config)
        {
            var context = config.GetScreenConfigurationContext<RSSVWorkOrderEntry,
                RSSVWorkOrder>();
            var conditions = context.Conditions.GetPack<Conditions>();
            ...
        }
    }
}