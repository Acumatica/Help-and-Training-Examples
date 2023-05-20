using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;
using PX.Data.BQL;
////////// The added code
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.SO.SOInvoiceEntry,
  PX.Objects.AR.ARInvoice>;
////////// The end of added code

namespace PhoneRepairShop
{
    public class SOInvoiceOrder_Workflow : PXGraphExtension<SOInvoiceEntry_Workflow,
        SOInvoiceEntry>
    {
		public const string ApproveDiscount = "Approve Discount";
		
        ////////// The added code
        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition DiscountEmpty => GetOrCreate(b =>
              b.FromBql<ARInvoice.curyDiscTot.IsEqual<decimal0>>());
        }
        #endregion
        ////////// The end of added code
		
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
				
			var approveDiscount = context.ActionDefinitions
			    .CreateNew(ApproveDiscount, a => a
				    .DisplayName("Approve Discount"));
				
            ////////// The added code				
            var conditions = context.Conditions.GetPack<Conditions>();
            ////////// The end of added code

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
					actions.Add(approveDiscount);
                })
				.WithFieldStates(fs =>
				{
					fs.Add<ARInvoice.status>(state =>
						state.SetComboValue(ARDocStatus_Postponed
						  .Postponed, "Postponed"));
				})
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
}
