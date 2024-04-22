using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ////////// The added code
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;
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
        ////////// The end of added code

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            ////////// The added code
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignee>(
                WorkOrders.Cache, null, true);
            ////////// The end of added code
        }

        ////////// The added code
        protected virtual void _(Events.RowSelected<
            RSSVWorkOrderToAssignFilter> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
            g => g.Assign);
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }
        ////////// The end of added code

        [PXHidden]
        public class RSSVWorkOrderToAssignFilter : PXBqlTable, IBqlTable
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