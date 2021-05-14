...

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        ...
		
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            RSSVWorkOrder row = e.Row;
            if (row == null) return;
            Assign.SetEnabled(row.Status == WorkOrderStatusConstants.ReadyForAssignment &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
            Complete.SetEnabled(row.Status == WorkOrderStatusConstants.Assigned &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);

            CreateInvoiceAction.SetVisible(WorkOrders.Current.Status ==
                WorkOrderStatusConstants.PendingPayment ||
                WorkOrders.Current.Status == WorkOrderStatusConstants.Completed
                && WorkOrders.Current.InvoiceNbr == null);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null);

            Payments.Cache.AllowSelect =
                row.Status == WorkOrderStatusConstants.Paid;
        }
		
		...
		
	}
	
}