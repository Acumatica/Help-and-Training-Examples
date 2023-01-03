using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    public class SOInvoiceOrder_Workflow : PXGraphExtension<SOInvoiceEntry_Workflow,
        SOInvoiceEntry>
    {
        public override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOInvoiceEntry, ARInvoice>());
        }

        protected virtual void Configure(WorkflowContext<SOInvoiceEntry,
            ARInvoice> context)
        {
            var repairCategory = context.Categories.CreateNew(
                ActionCategories.RepairCategoryID,
                category => category.DisplayName(
                ActionCategories.DisplayNames.RepairOrders));

            var viewOrder = context.ActionDefinitions
              .CreateExisting<SOInvoiceEntry_Extension>(g => g.ViewOrder,
                a => a.WithCategory(repairCategory));

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
                ////////// The added code
				.WithFieldStates(fs =>
				{
					fs.Add<ARInvoice.status>(state =>
						state.SetComboValue(ARDocStatus_Postponed
						  .Postponed, "Postponed"));
				})
                ////////// The end of added code
            );
        }
    }

    public static class ActionCategories
    {
        public const string RepairCategoryID = "Repair Orders Category";

        [PXLocalizable]
        public static class DisplayNames
        {
            public const string RepairOrders = "Repair Orders";
        }
    }

    ////////// The added code
    public class ARDocStatus_Postponed : ARDocStatus
    {
        public const string Postponed = "O";
        public class postponed : BqlType<IBqlString, string>.Constant<postponed>
        {
            public postponed()
                : base("O")
            {
            }
        }
    }
    ////////// The end of added code
}
