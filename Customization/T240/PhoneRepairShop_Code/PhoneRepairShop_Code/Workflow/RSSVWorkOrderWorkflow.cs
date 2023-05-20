using PX.Data.WorkflowAPI;
using PhoneRepairShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.BQL;
using PX.Data;
using PX.Objects.CS;
using System.CodeDom;
using PX.Data.BQL.Fluent;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry, PhoneRepairShop.RSSVWorkOrder>;
using PX.Objects.AR;
using PX.Objects.Common;

namespace PhoneRepairShop.Workflows
{
    public class RSSVWorkOrderWorkflow : PX.Data.PXGraphExtension<RSSVWorkOrderEntry>
    {
        // workflow works without checking active
        public static bool IsActive() => false;

        #region Constants
        public static class States
        {
            public const string OnHold = WorkOrderStatusConstants.OnHold;
            public const string ReadyForAssignment = WorkOrderStatusConstants.ReadyForAssignment;
            public const string PendingPayment = WorkOrderStatusConstants.PendingPayment;
            public const string Assigned = WorkOrderStatusConstants.Assigned;
            public const string Completed = WorkOrderStatusConstants.Completed;
            public const string Paid = WorkOrderStatusConstants.Paid;

            public class onHold : PX.Data.BQL.BqlString.Constant<onHold>
            {
                public onHold() : base(OnHold) { }
            }

            public class readyForAssignment : PX.Data.BQL.BqlString.Constant<readyForAssignment>
            {
                public readyForAssignment() : base(ReadyForAssignment) { }
            }

            public class pendingPayment : PX.Data.BQL.BqlString.Constant<pendingPayment>
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
            public Condition RequiresPrepayment => GetOrCreate(b => b.FromBql<
                Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>, Equal<True>>
            >());

            public Condition DoesNotRequirePrepayment => GetOrCreate(b => b.FromBql<
                Where<Selector<RSSVWorkOrder.serviceID, RSSVRepairService.prepayment>, Equal<False>>
            >());

        }

        #endregion Conditions

        public override void Configure(PXScreenConfiguration config)
        {

            var context = config.GetScreenConfigurationContext<RSSVWorkOrderEntry, RSSVWorkOrder>();
            var conditions = context.Conditions.GetPack<Conditions>();

            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            var otherCategory = commonCategories.Other;

            // start
            context.AddScreenConfigurationFor(screen =>
                screen
                    .StateIdentifierIs<RSSVWorkOrder.status>()
                    .AddDefaultFlow(flow =>
                        flow
                        .WithFlowStates(fss =>
                        {
                            fss.Add<States.onHold>(flowState =>
                            {
                                return flowState
                                    .IsInitial()
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.ReleaseFromHold, a => a.IsDuplicatedInToolbar());

                                    });
                            });
                            fss.Add<States.readyForAssignment>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
                                    })
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                                        actions.Add(g => g.Assign, a => a.IsDuplicatedInToolbar());
                                    });
                            });
                            fss.Add<States.pendingPayment>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
                                    })
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());

                                    });
                            });
                            fss.Add<States.assigned>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
                                    })
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.Complete, a => a.IsDuplicatedInToolbar());
                                    });
                            });
                            fss.Add<States.completed>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.dateCompleted>(state => state.IsDisabled());
                                    })
                                    .WithEventHandlers(handlers =>
                                    {
                                        handlers.Add(g => g.OnCloseDocument);
                                    });
                            });
                            fss.Add<States.paid>(flowState =>
                            {
                                return flowState
                                    .WithFieldStates(states =>
                                    {
                                        states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
                                        states.AddField<RSSVWorkOrder.dateCompleted>(state => state.IsDisabled());
                                    });
                            });
                        })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom<States.onHold>(ts =>
                        {
                            //add condition
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
                        transitions.AddGroupFrom<States.assigned>(ts =>
                        {
                            ts.Add(t => t.To<States.completed>().IsTriggeredOn(g => g.Complete));
                        });
                        transitions.AddGroupFrom<States.completed>(ts =>
                        {
                            ts.Add(t => t.To<States.paid>().IsTriggeredOn(g => g.OnCloseDocument));
                        });

                    }
                    ))
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.PutOnHold, c => c.WithCategory(processingCategory, g => g.ReleaseFromHold));
                        actions.Add(g => g.ReleaseFromHold, c => c.WithCategory(processingCategory));
                        actions.Add(g => g.Assign,
                            c => c.WithCategory(
                                processingCategory, g => g.PutOnHold)
                            .MassProcessingScreen<RSSVAssignProcess>()
                            .InBatchMode());
                        actions.Add(g => g.Complete, c => c.WithCategory(processingCategory, g => g.Assign)
                            .WithFieldAssignments(fas => fas.Add<RSSVWorkOrder.dateCompleted>(f => f.SetFromToday())));
                    })
                    .WithHandlers(handlers =>
                    {
                        handlers.Add(handler => handler
                            .WithTargetOf<ARInvoice>()
                            .OfEntityEvent<ARInvoice.Events>(e => e.CloseDocument)
                            .Is(g => g.OnCloseDocument)
                            .UsesPrimaryEntityGetter<
                                SelectFrom<RSSVWorkOrder>.
                                Where<RSSVWorkOrder.invoiceNbr.IsEqual<ARRegister.refNbr.FromCurrent>>>());
                    })
                    .WithCategories(categories =>
                    {
                        categories.Add(processingCategory);
                        categories.Add(otherCategory);
                    })
                );


        }
    }
}
