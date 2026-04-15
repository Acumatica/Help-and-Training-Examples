using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXCancel<RSSVWorkOrder> Cancel = null!;
        public 
            SelectFrom<RSSVWorkOrder>.
            // Inside the Where condition, use a fluent BQL statement 
            // that selects only the repair work orders with 
            // the Ready for Assignment status. 
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderEntry_Workflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
        }

        public PXFilter<MasterTable> MasterView;
        public PXFilter<DetailsTable> DetailsView;
                
        [Serializable]
        public class MasterTable : PXBqlTable, IBqlTable
        {
                
        }
                
        [Serializable]
        public class DetailsTable : PXBqlTable, IBqlTable
        {
                
        }

        ////////// The added code
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                graph => graph.Assign);
        }
        ////////// The end of added code
    }
}