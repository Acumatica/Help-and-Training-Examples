// Manage visibility and availability of the actions.
protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
{
  RSSVWorkOrder row = e.Row;
  if (row == null) return;
  AssignToMe.SetEnabled((row.Status == WorkOrderStatusConstants.ReadyForAssignment ||
    row.Status == WorkOrderStatusConstants.OnHold) && 
    WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
  AssignToMe.SetVisible(row.Assignee != PXAccess.GetContactID());
}