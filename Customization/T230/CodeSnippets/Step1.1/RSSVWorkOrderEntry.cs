public PXAction<RSSVWorkOrder> AssignToMe;
[PXButton]
[PXUIField(DisplayName = "Assign to Me", Enabled = true)]
protected virtual void assignToMe()
{
	// Get the current order from the cache.
	RSSVWorkOrder row = WorkOrders.Current;

	// Assign the contact ID associated with the current user
	row.Assignee = PXAccess.GetContactID();

	// Update the data record in the cache.
	WorkOrders.Update(row);

	// Trigger the Save action to save changes in the database.
	Actions.PressSave();
}