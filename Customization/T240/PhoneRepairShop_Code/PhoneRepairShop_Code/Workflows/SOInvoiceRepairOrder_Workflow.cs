using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;
using PX.Data.BQL;
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.SO.SOInvoiceEntry,
  PX.Objects.AR.ARInvoice>;
using PX.Objects.CS;

namespace PhoneRepairShop
{
    public class SOInvoiceRepairOrder_Workflow : 
        PXGraphExtension<SOInvoiceEntry_Workflow, SOInvoiceEntry>
    {
        public const string ApproveDiscount = "Approve Discount";

        public static class ActionCategories
        {
            public const string RepairCategoryID = "Repair Work Orders Category";

            [PXLocalizable]
            public static class DisplayNames
            {
                public const string RepairOrders = "Repair Work Orders";
            }
        }

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition DiscountEmpty => GetOrCreate(condition =>
              condition.FromBql<ARInvoice.curyDiscTot.IsEqual<decimal0>>());
        }
        #endregion

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOInvoiceEntry,
                                                           ARInvoice>());
        }

        protected static void Configure(WorkflowContext<SOInvoiceEntry,
                                                        ARInvoice> context)
        {
            var repairCategory = context.Categories.CreateNew(
                ActionCategories.RepairCategoryID,
                category => category.DisplayName(
                ActionCategories.DisplayNames.RepairOrders));

            #region Action Definitions
            var viewOrder = context.ActionDefinitions
              .CreateExisting<SOInvoiceEntry_Extension>(graph => graph.ViewOrder,
                action => action.WithCategory(repairCategory));
            var approveDiscount = context.ActionDefinitions
              .CreateNew(ApproveDiscount, action => action
                .DisplayName("Approve Discount"));
            #endregion

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
                            flowStates.UpdateSequence<ARDocStatus.HoldToBalance>(
                                seq =>
                            {
                                return seq.WithStates(states =>
                                {
                                    states.Add<ARDocStatus_Postponed.postponed>(
                                        flowState =>
                                    {
                                        return flowState
                                        .PlaceAfter<ARDocStatus.creditHold>()
                                        .IsSkippedWhen(conditions.DiscountEmpty)
                                        .WithActions(actions =>
                                        {
                                            actions.Add(approveDiscount,
                                                action => action
                                                .IsDuplicatedInToolbar()
                                                .WithConnotation(
                                                    ActionConnotation.Success));
                                        });
                                    });
                                });
                            });
                        })
                        .WithTransitions(transitions =>
                        {
                            transitions.AddGroupFrom<ARDocStatus_Postponed.postponed>(
                                transitionGroup =>
                            {
                                transitionGroup.Add(transition => transition
                                    .ToNext()
                                    .IsTriggeredOn(approveDiscount)
                                    .WithFieldAssignments(fields =>
                                        fields.Add<ARInvoice.discDate>(
                                            field => field.SetFromToday())));
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
