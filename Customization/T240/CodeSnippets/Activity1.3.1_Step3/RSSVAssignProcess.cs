using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ////////// The added code
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel = null!;
        public PXFilter<RSSVWorkOrderToAssignFilter> Filter = null!;
        public SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.IsEqual<
                RSSVWorkOrderEntry_Workflow.States.readyForAssignment>.
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
           FilteredBy<RSSVWorkOrderToAssignFilter> WorkOrders = null!;
        ////////// The end of added code

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(list =>
                AssignOrders(list, true));
            ////////// The added code
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignee>(
                WorkOrders.Cache, null, true);
            ////////// The end of added code
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
        public override bool IsDirty => false;
        ////////// The end of added code

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
    }
}