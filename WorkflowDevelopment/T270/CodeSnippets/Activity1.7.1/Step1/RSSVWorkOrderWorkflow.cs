using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry,
  PhoneRepairShop.RSSVWorkOrder>;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class RSSVWorkOrderWorkflow :
      PX.Data.PXGraphExtension<RSSVWorkOrderEntry>
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

        public class Conditions : Condition.Pack
        {
            public Condition RequiresPrepayment => GetOrCreate(b => b.FromBql<
              Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>,
              Equal<True>>>());

            public Condition DoesNotRequirePrepayment => GetOrCreate(b => b.FromBql<
              Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>,
              Equal<False>>>());
        }

        public sealed override void Configure (PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<RSSVWorkOrderEntry,
                                                           RSSVWorkOrder>());
        }

        protected static void Configure(WorkflowContext<RSSVWorkOrderEntry, 
                                                        RSSVWorkOrder> context)
        {
            var formAssign = context.Forms.Create("FormAssign", form =>
                form.Prompt("Assign").WithFields(fields =>
                {
                    fields.Add("Assignee", field => field
                        .WithSchemaOf<RSSVWorkOrder.assignee>()
                        .IsRequired()
                        .Prompt("Assignee"));
                }));

            var conditions = context.Conditions.GetPack<Conditions>();

            #region Categories
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            #endregion

            context.AddScreenConfigurationFor(screen =>
                screen
                .StateIdentifierIs<RSSVWorkOrder.status>()
                .AddDefaultFlow(flow => flow
                    .WithFlowStates(fss =>
                    {
                        fss.Add<States.onHold>(flowState =>
                        {
                            return flowState
                            .IsInitial()
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ReleaseFromHold, a => a
                                .IsDuplicatedInToolbar()
                                .WithConnotation(ActionConnotation.Success));
                            });
                        });
                        fss.Add<States.readyForAssignment>(flowState =>
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
                                  actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                                  actions.Add(g => g.Assign, a => a
                                      .IsDuplicatedInToolbar()
                                      .WithConnotation(ActionConnotation.Success));
                              });
                        });
                        fss.Add<States.pendingPayment>(flowState =>
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
                                actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                              });
                        });
						////////// The modified code
                        fss.Add<States.assigned>(flowState =>
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
                                // Add the action to the configuration
                                // of the Assigned state
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.Complete, a => a
                                      .IsDuplicatedInToolbar()
                                      .WithConnotation(ActionConnotation.Success));
                                });
                        });
						////////// The end of modified code
                    })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom<States.onHold>(ts =>
                        {
                            ts.Add(t => t.To<States.readyForAssignment>()
                                .IsTriggeredOn(g => g.ReleaseFromHold)
                                .When(conditions.DoesNotRequirePrepayment));
                            ts.Add(t => t.To<States.pendingPayment>()
                                .IsTriggeredOn(g => g.ReleaseFromHold)
                                .When(conditions.RequiresPrepayment));
                        });
                        transitions.AddGroupFrom<States.readyForAssignment>(ts =>
                        {
                            ts.Add(t => t.To<States.onHold>().IsTriggeredOn(g => g.PutOnHold));
                            ts.Add(t => t.To<States.assigned>().IsTriggeredOn(g => g.Assign));
                        });
                        transitions.AddGroupFrom<States.pendingPayment>(ts =>
                        {
                            ts.Add(t => t.To<States.onHold>().IsTriggeredOn(g => g.PutOnHold));
                        });
                    })
                )
                .WithCategories(categories =>
                {
                    categories.Add(processingCategory);
                })
                .WithActions(actions => 
                {
                    actions.Add(g => g.ReleaseFromHold, c => c
                      .WithCategory(processingCategory));
                    actions.Add(g => g.PutOnHold, c => c
                      .WithCategory(processingCategory));
                    actions.Add(g => g.Assign, c => c
                      .WithCategory(processingCategory)
                      .WithForm(formAssign)
                      .WithFieldAssignments(fields => {
                      fields.Add<RSSVWorkOrder.assignee>(f =>
                        f.SetFromFormField(formAssign, "Assignee"));
                      }));
                    ////////// The added code
                    actions.Add(g => g.Complete, c => c
                      .WithCategory(processingCategory, Placement.Last)
                      .WithFieldAssignments(fas => fas
                        .Add<RSSVWorkOrder.dateCompleted>(f =>
                          f.SetFromToday())));
                    ////////// The end of added code
                })
                .WithForms(forms => forms.Add(formAssign))
            );
        }
    }
}