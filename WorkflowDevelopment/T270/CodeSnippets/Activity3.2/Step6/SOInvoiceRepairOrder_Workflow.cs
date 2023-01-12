using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;
using PX.Data.BQL;
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.SO.SOInvoiceEntry,
  PX.Objects.AR.ARInvoice>;

namespace PhoneRepairShop
{
    public class SOInvoiceOrder_Workflow : PXGraphExtension<SOInvoiceEntry_Workflow,
        SOInvoiceEntry>
    {
        public const string ApproveDiscount = "Approve Discount";
        
        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition DiscountEmpty => GetOrCreate(b =>
              b.FromBql<ARInvoice.curyDiscTot.IsEqual<decimal0>>());
        }
        #endregion
        
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
                    
            var conditions = context.Conditions.GetPack<Conditions>();

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
                            flowStates.UpdateSequence<ARDocStatus.HoldToBalance>(seq =>
                            {
                                return seq.WithStates(states =>
                                {
                                    states.Add<ARDocStatus_Postponed.postponed>(flowState =>
                                    {
                                        return flowState
                                            .PlaceAfter<ARDocStatus.creditHold>()
                                            .IsSkippedWhen(conditions.DiscountEmpty)
                                            .WithActions(actions =>
                                            {
                                                actions.Add(approveDiscount, a => a
                                                  .IsDuplicatedInToolbar()
                                                  .WithConnotation(ActionConnotation.Success));
                                            });
                                    });
                                });
                            });
                        })
                        ////////// The added code
						.WithTransitions(transitions =>
						{
							transitions.AddGroupFrom<ARDocStatus_Postponed.postponed>(ts =>
							{
								ts.Add(t => t
									.ToNext()
									.IsTriggeredOn(approveDiscount)
									.WithFieldAssignments(fass => 
										fass.Add<ARInvoice.discDate>(f => f.SetFromToday())));
							});
						});
                        ////////// The end of added code
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
