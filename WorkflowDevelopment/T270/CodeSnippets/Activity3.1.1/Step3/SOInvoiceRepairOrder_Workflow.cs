using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;

namespace PhoneRepairShop
{
    public class SOInvoiceRepairOrder_Workflow : 
        PXGraphExtension<SOInvoiceEntry_Workflow, SOInvoiceEntry>
    {
        ////////// The added code
        public static class ActionCategories
        {
            public const string RepairCategoryID = "Repair Work Orders Category";

            [PXLocalizable]
            public static class DisplayNames
            {
                public const string RepairOrders = "Repair Work Orders";
            }
        }
        ////////// The end of added code

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOInvoiceEntry,
                                                           ARInvoice>());
        }

        protected static void Configure(WorkflowContext<SOInvoiceEntry,
                                                        ARInvoice> context)
        {
            ////////// The added code
            var repairCategory = context.Categories.CreateNew(
                ActionCategories.RepairCategoryID,
                category => category.DisplayName(
                ActionCategories.DisplayNames.RepairOrders));

            var viewOrder = context.ActionDefinitions
              .CreateExisting<SOInvoiceEntry_Extension>(graph => graph.ViewOrder,
                action => action.WithCategory(repairCategory));

            context.UpdateScreenConfigurationFor(screen => screen
                .UpdateDefaultFlow(flow =>
                {
                    return flow
                        .WithFlowStates(flowStates =>
                        {
                            flowStates.Update<ARDocStatus.open>(flowState =>
                            {
                                return flowState.WithActions(actions =>
                                    actions.Add(viewOrder));
                            });
                        });
                })
                .WithCategories(categories =>
                {
                    categories.Add(repairCategory);
                })
                .WithActions(actions =>
                {
                    actions.Add(viewOrder);
                })
            );
            ////////// The end of added code
        }
    }
}
