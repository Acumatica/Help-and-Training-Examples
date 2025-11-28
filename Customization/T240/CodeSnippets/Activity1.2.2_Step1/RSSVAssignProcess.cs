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

        /////////// The added code
        public static void AssignOrders(List<RSSVWorkOrder> list,
            bool isMassProcess = false)
        {
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                
            // The processing method uses the error handling
            // and progress tracking functionality of the PXProcessing class.
            PXProcessing<RSSVWorkOrder>.ProcessRecords(list, isMassProcess,
                workOrder =>
                {
                    workOrderEntry.Clear();
                    workOrderEntry.WorkOrders.Current = workOrder;
                    // If the assignee is not specified,
                    // specify the default employee.
                    if (workOrder.Assignee == null)
                    {
                        // Retrieve the record with the default setting
                        RSSVSetup setupRecord =
                            workOrderEntry.AutoNumSetup.Current;
                        workOrder.Assignee = setupRecord.DefaultEmployee;
                    }
                    // Assign the work order in the cache.
                    workOrderEntry.Assign.Press();
                });
        }
        /////////// The end of added code
    }
}