using System;
using PX.Data;
////////// The added code
using PhoneRepairShop.Workflows;
using PX.Data.BQL.Fluent;
////////// The end of added code

namespace PhoneRepairShop
{
    ////////// The added code
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXCancel<RSSVWorkOrder> Cancel;
        public SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderWorkflow.States.readyForAssignment>>.
            ProcessingView WorkOrders;

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
    }
    ////////// The end of added code
}