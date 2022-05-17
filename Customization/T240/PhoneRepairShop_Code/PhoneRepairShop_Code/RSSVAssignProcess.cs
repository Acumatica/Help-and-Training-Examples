using System;
using PX.Data;
using PhoneRepairShop.Workflows;
using PX.TM;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using System.Linq;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        /*public PXCancel<RSSVWorkOrder> Cancel;
        public SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderWorkflow.States.readyForAssignment>>.
            ProcessingView WorkOrders;*/

        public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
        public SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.IsEqual<
                RSSVWorkOrderWorkflow.States.readyForAssignment>.
                And<RSSVWorkOrder.timeWithoutAction.IsGreaterEqual<
                    RSSVWorkOrderToAssignFilter.timeWithoutAction.
                        FromCurrent>.
                And<RSSVWorkOrder.priority.IsEqual<
                    RSSVWorkOrderToAssignFilter.priority.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.priority.FromCurrent.
                        IsNull>>.
                And<RSSVWorkOrder.serviceID.IsEqual<
                    RSSVWorkOrderToAssignFilter.serviceID.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.serviceID.FromCurrent.
                        IsNull>>>>.
            OrderBy<RSSVWorkOrder.timeWithoutAction.Desc,
                RSSVWorkOrder.priority.Desc>.
            ProcessingView.
            FilteredBy<RSSVWorkOrderToAssignFilter> WorkOrders;
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }
        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        /*protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                g => g.Assign);
        }*/
        protected virtual void _(Events.RowSelected<
            RSSVWorkOrderToAssignFilter> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                g => g.Assign);
        }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [Owner(IsDBField = false, DisplayName = "Default Assignee")]
        [PXDBScalar(typeof(SelectFrom<OwnerAttribute.Owner>.
            LeftJoin<RSSVEmployeeWorkOrderQty>.
            On<OwnerAttribute.Owner.contactID.IsEqual<
                RSSVEmployeeWorkOrderQty.userID>>.
            Where<OwnerAttribute.Owner.acctCD.IsNotNull>.
            OrderBy<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders.Asc,
                RSSVEmployeeWorkOrderQty.lastModifiedDateTime.Asc>.
            SearchFor<OwnerAttribute.Owner.contactID>))]
        protected virtual void _(
            Events.CacheAttached<RSSVWorkOrder.defaultAssignee> e)
        { }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [Owner(IsDBField = false, DisplayName = "Assign To")]
        [PXUnboundDefault(typeof(RSSVWorkOrder.assignee.When<
            RSSVWorkOrder.assignee.IsNotNull>.
            Else<RSSVWorkOrder.defaultAssignee>))]
        protected virtual void _(
            Events.CacheAttached<RSSVWorkOrder.assignTo> e)
        { }

        protected virtual void _(Events.FieldSelecting<RSSVWorkOrder,
                         RSSVWorkOrder.nbrOfAssignedOrders> e)
        {
            if (e.Row == null) return;
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                SelectFrom<RSSVEmployeeWorkOrderQty>.
                Where<RSSVEmployeeWorkOrderQty.userID.IsEqual<@P.AsInt>>.
                    View.Select(this, e.Row.AssignTo);
            if (employeeNbrOfOrders != null)
            {
                e.ReturnValue = employeeNbrOfOrders.NbrOfAssignedOrders.
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