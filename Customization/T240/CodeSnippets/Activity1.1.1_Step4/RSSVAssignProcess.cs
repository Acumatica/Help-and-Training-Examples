using System;
using PX.Data;
////////// The added code
using PX.Data.BQL.Fluent;
////////// The end of added code

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ////////// The added code
        public PXCancel<RSSVWorkOrder> Cancel = null!;
        ////////// The end of added code
        public 
            SelectFrom<RSSVWorkOrder>.
            // Inside the Where condition, use a fluent BQL statement 
            // that selects only the repair work orders with 
            // the Ready for Assignment status. 
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderEntry_Workflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;

        ////////// The added code
        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
        }
        ////////// The end of added code

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
    }
}