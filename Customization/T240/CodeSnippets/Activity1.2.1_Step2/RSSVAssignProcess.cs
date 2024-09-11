using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXCancel<RSSVWorkOrder> Cancel = null!;
        public SelectFrom<RSSVWorkOrder>.
            // Inside the Where condition, use a fluent BQL statement 
            // that selects only the repair work orders with 
            // the Ready for Assignment status. 
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderWorkflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
        }

        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                g => g.Assign);
        }
        ////////// The added code
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
            public virtual string? Priority { get; set; }
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
        ////////// The end of added code
    }
}