using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry,
  PhoneRepairShop.RSSVWorkOrder>;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class RSSVWorkOrderEntry_Workflow : PXGraphExtension<RSSVWorkOrderEntry>
    {
        #region Constants
        public static class States
        {
            public const string OnHold = WorkOrderStatusConstants.OnHold;
            public const string ReadyForAssignment =
              WorkOrderStatusConstants.ReadyForAssignment;
            public const string PendingPayment =
              WorkOrderStatusConstants.PendingPayment;
            public const string Assigned = WorkOrderStatusConstants.Assigned;
            public const string Completed = WorkOrderStatusConstants.Completed;
            public const string Paid = WorkOrderStatusConstants.Paid;

            public class onHold : PX.Data.BQL.BqlString.Constant<onHold>
            {
                public onHold() : base(OnHold) { }
            }

            public class readyForAssignment :
              PX.Data.BQL.BqlString.Constant<readyForAssignment>
            {
                public readyForAssignment() : base(ReadyForAssignment) { }
            }

            public class pendingPayment :
              PX.Data.BQL.BqlString.Constant<pendingPayment>
            {
                public pendingPayment() : base(PendingPayment) { }
            }

            public class assigned : PX.Data.BQL.BqlString.Constant<assigned>
            {
                public assigned() : base(Assigned) { }
            }

            public class completed : PX.Data.BQL.BqlString.Constant<completed>
            {
                public completed() : base(Completed) { }
            }

            public class paid : PX.Data.BQL.BqlString.Constant<paid>
            {
                public paid() : base(Paid) { }
            }
        }
        #endregion

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition RequiresPrepayment => GetOrCreate(condition =>
                condition.FromBql<Where<RSSVRepairService.prepayment
                    .FromSelectorOf<RSSVWorkOrder.serviceID>.IsEqual<True>>>());
        }
        #endregion

        protected static void Configure(WorkflowContext<RSSVWorkOrderEntry,
                                                RSSVWorkOrder> context)
        {
            // Define the Assign dialog box
            var formAssign = context.Forms.Create("FormAssign", form =>
                form.Prompt("Assign").WithFields(fields =>
                {
                    fields.Add("Assignee", field => field
                       .WithSchemaOf<RSSVWorkOrder.assignee>()
                       .IsRequired()
                       .Prompt("Assignee"));
                }));

            #region Categories
                var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            #endregion

            // Create an instance of the Conditions class
            var conditions = context.Conditions.GetPack<Conditions>();

            context.AddScreenConfigurationFor(screen => screen
                .StateIdentifierIs<RSSVWorkOrder.status>()
                .AddDefaultFlow(flow => flow
                    .WithFlowStates(flowStates =>
                    {
                        flowStates.Add<States.onHold>(flowState =>
                            GetOnHoldBehavior(flowState));
                        flowStates.Add<States.readyForAssignment>(flowState =>
                            GetReadyForAssignmentBehavior(flowState));
                        flowStates.Add<States.pendingPayment>(flowState =>
                            GetPendingPaymentBehavior(flowState));
                        ////////// The added code
                        flowStates.Add<States.assigned>(flowState =>
                            GetAssignedBehavior(flowState));
                        ////////// The end of added code
                    })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom<States.onHold>(transitionGroup =>
                        {
                            transitionGroup.Add(transition => 
                                transition.To<States.readyForAssignment>()
                                .IsTriggeredOn(graph => graph.ReleaseFromHold)
                                .When(!conditions.RequiresPrepayment));
                            transitionGroup.Add(transition => 
                                transition.To<States.pendingPayment>()
                                .IsTriggeredOn(graph => graph.ReleaseFromHold)
                                .When(conditions.RequiresPrepayment));
                        });
                        transitions.AddGroupFrom<States.readyForAssignment>(
                            transitionGroup =>
                        {
                            transitionGroup.Add(transition =>
                                transition.To<States.onHold>()
                                .IsTriggeredOn(graph => graph.PutOnHold));
                            ////////// The added code
                            transitionGroup.Add(transition =>
                                transition.To<States.assigned>()
                                .IsTriggeredOn(graph => graph.Assign));
                            ////////// The end of added code
                        });
                        transitions.AddGroupFrom<States.pendingPayment>(
                            transitionGroup =>
                        {
                            transitionGroup.Add(transition =>
                                transition.To<States.onHold>()
                                .IsTriggeredOn(graph => graph.PutOnHold));
                        });
                    }))
                .WithCategories(categories =>
                {
                    categories.Add(processingCategory);
                })
                .WithActions(actions =>
                {
                    actions.Add(graph => graph.ReleaseFromHold,
                      action => action.WithCategory(processingCategory));
                    actions.Add(graph => graph.PutOnHold, action => action
                      .WithCategory(processingCategory));
                    actions.Add(graph => graph.Assign, action => action
                      .WithCategory(processingCategory)
                      .WithForm(formAssign)
                      .WithFieldAssignments(fields => {
                          fields.Add<RSSVWorkOrder.assignee>(field =>
                            field.SetFromFormField(formAssign, "Assignee"));
                      }));
                })
                .WithForms(forms => forms.Add(formAssign))
            );
        }

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<RSSVWorkOrderEntry,
                RSSVWorkOrder>());
        }

        #region Workflow States
        private static BaseFlowStep.IConfigured GetOnHoldBehavior(
            FlowState.INeedAnyFlowStateConfig flowState)
        {
            return flowState
            .IsInitial()
            .WithActions(actions =>
            {
                actions.Add(graph => graph.ReleaseFromHold,
                    action => action.IsDuplicatedInToolbar()
                        .WithConnotation(
                            ActionConnotation.Success));
            });
        }

        private static BaseFlowStep.IConfigured GetReadyForAssignmentBehavior(
            FlowState.INeedAnyFlowStateConfig flowState)
        {
            return flowState
            .WithFieldStates(states =>
            {
                states.AddField<RSSVWorkOrder.customerID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.serviceID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.deviceID>(state
                    => state.IsDisabled());
            })
            .WithActions(actions =>
            {
                actions.Add(graph => graph.PutOnHold,
                    action => action.IsDuplicatedInToolbar());
                ////////// The added code
                actions.Add(graph => graph.Assign,
                    action => action.IsDuplicatedInToolbar()
                    .WithConnotation(ActionConnotation.Success));
                ////////// The end of added code
            });
        }

        private static BaseFlowStep.IConfigured GetPendingPaymentBehavior(
            FlowState.INeedAnyFlowStateConfig flowState)
        {
            return flowState
            .WithFieldStates(states =>
            {
                states.AddField<RSSVWorkOrder.customerID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.serviceID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.deviceID>(state
                    => state.IsDisabled());
            })
            .WithActions(actions =>
            {
                actions.Add(graph => graph.PutOnHold, 
                    action => action.IsDuplicatedInToolbar());
            });
        }

        ////////// The added code
        private static BaseFlowStep.IConfigured GetAssignedBehavior(
            FlowState.INeedAnyFlowStateConfig flowState)
        {
            return flowState
            .WithFieldStates(states =>
            {
                states.AddField<RSSVWorkOrder.customerID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.serviceID>(state
                    => state.IsDisabled());
                states.AddField<RSSVWorkOrder.deviceID>(state
                    => state.IsDisabled());
            });
        }
        ////////// The end of added code
        #endregion
    }
}
