using PX.Data.WorkflowAPI;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        ...

        //Event handler for a workflow event
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;

        public PXAction<RSSVWorkOrder> Assign;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual void assign()
        {
            // Get the current order from the cache.
            RSSVWorkOrder row = WorkOrders.Current;

            // If an Assignee has not been specified,
            // change the Assignee box value to the default employee value.
            if (row.Assignee == null)
                row.Assignee = AutoNumSetup.Current.DefaultEmployee;

            // Change the order status to Assigned.
            // row.Status = WorkOrderStatusConstants.Assigned;

            // Update the data record in the cache.
            WorkOrders.Update(row);

            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter) => adapter.Get();

    }
}