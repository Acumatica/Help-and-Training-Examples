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

            #region Categories
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            #endregion

            context.AddScreenConfigurationFor(screen => screen
                .StateIdentifierIs<RSSVWorkOrder.status>()
                    .AddDefaultFlow(flow => ...)
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