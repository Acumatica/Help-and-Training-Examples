using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry,
  PhoneRepairShop.RSSVWorkOrder>;
using static PhoneRepairShop.RSSVWorkOrder;

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

            public Condition HasInvoice => GetOrCreate(condition => 
                condition.FromBql<Where<RSSVWorkOrder.invoiceNbr.IsNotNull>>());
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
                        flowStates.Add<States.assigned>(flowState =>
                            GetAssignedBehavior(flowState));
                        flowStates.Add<States.completed>(flowState =>
                            GetCompletedBehavior(flowState));
                        flowStates.Add<States.paid>(flowState =>
                            GetPaidBehavior(flowState));
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
                            transitionGroup.Add(transition =>
                                transition.To<States.assigned>()
                                .IsTriggeredOn(graph => graph.Assign));
                        });
                        transitions.AddGroupFrom<States.pendingPayment>(
                            transitionGroup =>
                        {
                            transitionGroup.Add(transition =>
                                transition.To<States.onHold>()
                                .IsTriggeredOn(graph => graph.PutOnHold));
                            transitionGroup.Add(transition =>
                                transition.To<States.readyForAssignment>()
                                .IsTriggeredOn(graph => graph.OnInvoiceGotPrepaid));
                        });
                        transitions.AddGroupFrom<States.assigned>(
                            transitionGroup =>
                        {
                            transitionGroup.Add(transition =>
                                transition.To<States.completed>()
                                .IsTriggeredOn(graph => graph.Complete));
                        });
                        transitions.AddGroupFrom<States.completed>(
                            transitionGroup =>
                        {
                            transitionGroup.Add(transition => 
                                transition.To<States.paid>()
                              .IsTriggeredOn(graph => graph.OnCloseDocument));
                        });
                    }))
                .WithHandlers(handlers =>
                {
                    handlers.Add(handler => handler
                        .WithTargetOf<ARInvoice>()
                        .OfEntityEvent<ARInvoice.Events>(
                            workflowEvent => workflowEvent.CloseDocument)
                            .Is(graph => graph.OnCloseDocument)
                            .UsesPrimaryEntityGetter<
                                SelectFrom<RSSVWorkOrder>.
                                Where<RSSVWorkOrder.invoiceNbr
                                  .IsEqual<ARRegister.refNbr.FromCurrent>>>());
                    handlers.Add(handler => handler
                        .WithTargetOf<ARRegister>()
                        .OfEntityEvent<RSSVWorkOrder.WorkflowEvents>(
                            workflowEvent => workflowEvent.InvoiceGotPrepaid)
                            .Is(graph => graph.OnInvoiceGotPrepaid)
                            .UsesPrimaryEntityGetter<
                                SelectFrom<RSSVWorkOrder>.
                                Where<RSSVWorkOrder.invoiceNbr
                                .IsEqual<ARRegister.refNbr.FromCurrent>>>());
                })
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
                    actions.Add(graph => graph.Complete, action => action
                        .WithCategory(processingCategory, Placement.Last)
                        .WithFieldAssignments(fields => fields
                            .Add<RSSVWorkOrder.dateCompleted>(field =>
                                field.SetFromToday())));
                    actions.Add(graph => graph.CreateInvoiceAction,
                        action => action.WithCategory(processingCategory)
                        .IsDisabledWhen(conditions.HasInvoice));
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
                actions.Add(graph => graph.Assign,
                    action => action.IsDuplicatedInToolbar()
                    .WithConnotation(ActionConnotation.Success));
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
            })
            .WithActions(actions =>
            {
                actions.Add(graph => graph.CreateInvoiceAction,
                    action => action.IsDuplicatedInToolbar()
                    .WithConnotation(ActionConnotation.Success));
            })
            .WithEventHandlers(handlers =>
            {
                handlers.Add(graph => graph.OnInvoiceGotPrepaid);
            });
        }

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
            })
            .WithActions(actions =>
            {
                actions.Add(graph => graph.Complete, action => action
                  .IsDuplicatedInToolbar()
                  .WithConnotation(ActionConnotation.Success));
            });
        }

        private static BaseFlowStep.IConfigured GetCompletedBehavior(
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
                actions.Add(graph => graph.CreateInvoiceAction,
                    action => action.IsDuplicatedInToolbar()
                    .WithConnotation(ActionConnotation.Success));
            })
            .WithEventHandlers(handlers =>
            {
                handlers.Add(graph => graph.OnCloseDocument);
            });
        }

        private static BaseFlowStep.IConfigured GetPaidBehavior(
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
        #endregion
    }

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class RSSVWorkOrderEntry_Workflow_Extension :
        PXGraphExtension<RSSVWorkOrderEntry_Workflow, RSSVWorkOrderEntry>
    {
        #region Constants 
        public static class OrderTypes
        {
            public const string Simple = WorkOrderTypeConstants.Simple;
            public const string Standard = WorkOrderTypeConstants.Standard;
            public const string Awaiting = WorkOrderTypeConstants.Awaiting;

            public class simple : PX.Data.BQL.BqlString.Constant<simple>
            {
                public simple() : base(Simple) { }
            }

            public class standard : PX.Data.BQL.BqlString.Constant<standard>
            {
                public standard() : base(Standard) { }
            }

            public class awaiting : PX.Data.BQL.BqlString.Constant<awaiting>
            {
                public awaiting() : base(Awaiting) { }
            }
        }
        #endregion

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<RSSVWorkOrderEntry,
                                                           RSSVWorkOrder>());
        }

        protected static void Configure(WorkflowContext<RSSVWorkOrderEntry,
                                                         RSSVWorkOrder> context)
        {
            context.UpdateScreenConfigurationFor(screen => screen
                .FlowTypeIdentifierIs<RSSVWorkOrder_Extension.usrOrderType>()
                .WithFlows(flows => flows
                    .Add<OrderTypes.simple>(flow =>
                        getSimpleBehavior(flow)))
            );
        }

        private static Workflow.IConfigured getSimpleBehavior(
            Workflow.INeedStatesFlow flowState)
        {
            return flowState
            .WithFlowStates(states =>
            {
                states.Add<RSSVWorkOrderEntry_Workflow.States.onHold>(flowState =>
                {
                    return flowState
                      .IsInitial()
                      .WithActions(actions =>
                      {
                          actions.Add(g => g.Complete, a => a
                          .IsDuplicatedInToolbar()
                          .WithConnotation(ActionConnotation.Success));
                      });
                });
                states.Add<RSSVWorkOrderEntry_Workflow.States.completed>(flowState =>
                {
                    return flowState
                        .WithFieldStates(fieldstates =>
                        {
                            fieldstates.AddField<RSSVWorkOrder.customerID>(state =>
                                state.IsDisabled());
                            fieldstates.AddField<RSSVWorkOrder.serviceID>(state =>
                                state.IsDisabled());
                            fieldstates.AddField<RSSVWorkOrder.deviceID>(state =>
                                state.IsDisabled());
                        })
                        .WithActions(actions =>
                        {
                            actions.Add(g => g.CreateInvoiceAction, a => a
                            .IsDuplicatedInToolbar()
                            .WithConnotation(ActionConnotation.Success));
                        })
                        .WithEventHandlers(handlers =>
                        {
                            handlers.Add(g => g.OnCloseDocument);
                        });
                });
                states.Add<RSSVWorkOrderEntry_Workflow.States.paid>(flowState =>
                {
                    return flowState
                        .WithFieldStates(fieldstates =>
                        {
                            fieldstates.AddField<RSSVWorkOrder.customerID>(state =>
                                state.IsDisabled());
                            fieldstates.AddField<RSSVWorkOrder.serviceID>(state =>
                                state.IsDisabled());
                            fieldstates.AddField<RSSVWorkOrder.deviceID>(state =>
                                state.IsDisabled());
                        });
                });
            })
            .WithTransitions(transitions =>
            {
                transitions.AddGroupFrom<RSSVWorkOrderEntry_Workflow.States.onHold>(
                    transitionGroup =>
                {
                    transitionGroup.Add(transition => transition
                        .To<RSSVWorkOrderEntry_Workflow.States.completed>()
                        .IsTriggeredOn(graph => graph.Complete));
                });
                transitions.AddGroupFrom<RSSVWorkOrderEntry_Workflow.States.completed>(
                    transitionGroup =>
                {
                    transitionGroup.Add(transition => transition
                        .To<RSSVWorkOrderEntry_Workflow.States.paid>()
                        .IsTriggeredOn(graph => graph.OnCloseDocument));
                });
            });
        }
    }
}
