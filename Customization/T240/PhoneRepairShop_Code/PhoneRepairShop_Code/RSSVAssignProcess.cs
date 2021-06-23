using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PhoneRepairShop.Workflows;
using PX.TM;
using System.Collections.Generic;
using PX.Data.BQL;
using System.Linq;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;
        public PXFilteredProcessing<RSSVWorkOrder, RSSVWorkOrderToAssignFilter,
            Where<RSSVWorkOrder.status.IsEqual<
                RSSVWorkOrderWorkflow.States.readyForAssignment>.
                And<RSSVWorkOrder.timeWithoutAction.IsGreaterEqual<
                    RSSVWorkOrderToAssignFilter.timeWithoutAction.FromCurrent>.
                    And<RSSVWorkOrder.priority.IsEqual<
                        RSSVWorkOrderToAssignFilter.priority.FromCurrent>.
                        Or<RSSVWorkOrderToAssignFilter.priority.FromCurrent.IsNull>>.
                    And<RSSVWorkOrder.serviceID.IsEqual<
                        RSSVWorkOrderToAssignFilter.serviceID.FromCurrent>.
                        Or<RSSVWorkOrderToAssignFilter.serviceID.FromCurrent.IsNull>>>>,
            OrderBy<Desc<RSSVWorkOrder.timeWithoutAction,
                RSSVWorkOrder.priority.Desc>>> WorkOrders;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(AssignOrders);
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }

        public static void AssignOrders(List<RSSVWorkOrder> orders)
        {
            // The result set to run the report on.
            PXReportResultset assignedOrders =
                new PXReportResultset(typeof(RSSVWorkOrder));

            RSSVWorkOrderEntry graph =
                PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            foreach (RSSVWorkOrder order in orders)
            {
                try
                {
                    //Change the assignee to the value selected on the form
                    order.Assignee = order.AssignTo;
                    graph.Clear();
                    graph.AssignOrder(order, true);

                    // Add to the result set the order 
                    // that has been successfully assigned.
                    if (order.Status == WorkOrderStatusConstants.Assigned)
                    {
                        assignedOrders.Add(order);
                    }
                }
                catch (Exception e)
                {
                    PXProcessing<RSSVWorkOrder>.SetError(
                        orders.IndexOf(order), e);
                }
            }

            if (assignedOrders.GetRowCount() > 0)
            {
                throw new PXReportRequiredException(assignedOrders, "RS601000",
                                                    Messages.ReportRS601000Title);
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [Owner(IsDBField = false, DisplayName = "Default Assignee")]
        [PXDBScalar(typeof(SelectFrom<OwnerAttribute.Owner>.
            LeftJoin<RSSVEmployeeWorkOrderQty>.
            On<OwnerAttribute.Owner.contactID.IsEqual<
                RSSVEmployeeWorkOrderQty.userid>>.
            Where<OwnerAttribute.Owner.acctCD.IsNotNull>.
            OrderBy<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders.Asc,
                RSSVEmployeeWorkOrderQty.lastModifiedDateTime.Asc>.
            SearchFor<OwnerAttribute.Owner.contactID>))]
        protected virtual void RSSVWorkOrder_DefaultAssignee_CacheAttached(
            PXCache sender)
        { }

        [Owner(IsDBField = false, DisplayName = "Assign To")]
        [PXUnboundDefault(typeof(RSSVWorkOrder.assignee.When<
            RSSVWorkOrder.assignee.IsNotNull>.
            Else<RSSVWorkOrder.defaultAssignee>))]
        protected virtual void RSSVWorkOrder_AssignTo_CacheAttached(
            PXCache sender)
        {
        }

        protected virtual void _(Events.FieldSelecting<RSSVWorkOrder,
                         RSSVWorkOrder.nbrOfAssignedOrders> e)
        {
            if (e.Row == null) return;
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                SelectFrom<RSSVEmployeeWorkOrderQty>.
                Where<RSSVEmployeeWorkOrderQty.userid.IsEqual<@P.AsInt>>.
                    View.Select(this, e.Row.AssignTo);
            if (employeeNbrOfOrders != null)
            {
                e.ReturnValue =  employeeNbrOfOrders.NbrOfAssignedOrders.
                    GetValueOrDefault();
            }
            else
            {
                e.ReturnValue = 0;
            }
        }

        [PXHidden]
        public class RSSVWorkOrderToAssignFilter : IBqlTable
        {
            #region Priority
            [PXString(1, IsFixed = true)]
            [PXUIField(DisplayName = "Priority")]
            [PXStringList(
                new string[]
                {
                    WorkOrderPriorityConstants.High,
                    WorkOrderPriorityConstants.Medium,
                    WorkOrderPriorityConstants.Low
                },
                new string[]
                {
                    Messages.High,
                    Messages.Medium,
                    Messages.Low
                })]
            public virtual string Priority { get; set; }
            public abstract class priority :
                PX.Data.BQL.BqlString.Field<priority>
            { }
            #endregion

            #region TimeWithoutAction
            [PXInt]
            [PXUnboundDefault(0)]
            [PXUIField(DisplayName = "Minimum Number of Days Unassigned")]
            public virtual int? TimeWithoutAction { get; set; }
            public abstract class timeWithoutAction :
                PX.Data.BQL.BqlInt.Field<timeWithoutAction>
            { }
            #endregion
            #region ServiceID
            [PXInt()]
            [PXUIField(DisplayName = "Service")]
            [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
                typeof(RSSVRepairService.serviceCD),
                typeof(RSSVRepairService.description),
                SubstituteKey = typeof(RSSVRepairService.serviceCD),
                DescriptionField = typeof(RSSVRepairService.description))]
            public virtual int? ServiceID { get; set; }
            public abstract class serviceID :
                PX.Data.BQL.BqlInt.Field<serviceID>
            { }
            #endregion
        }
    }
}