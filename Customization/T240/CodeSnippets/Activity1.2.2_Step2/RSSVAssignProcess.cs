using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;

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
                IsEqual<RSSVWorkOrderEntry_Workflow.States.readyForAssignment>>.
            ProcessingView WorkOrders = null!;
        ////////// The modified code
        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(list =>
                AssignOrders(list, true));
        }
        ////////// The end of modified code
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

        public static void AssignOrders(List<RSSVWorkOrder> list,
            bool isMassProcess = false)
        {
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
        
            // Define the processing method. You will use the error handling
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
    }
}