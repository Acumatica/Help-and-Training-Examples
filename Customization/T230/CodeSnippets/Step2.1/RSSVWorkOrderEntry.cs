public PXAction<RSSVWorkOrder> UpdateItemPrices;
[PXButton(CommitChanges = true)]
[PXUIField(DisplayName = "Update Prices", Enabled = true)]
protected virtual void updateItemPrices()
{
	var order = WorkOrders.Current;
	if (order.ServiceID != null && order.DeviceID != null)
	{
		var repairItems = RepairItems.Select();
		foreach (RSSVWorkOrderItem item in repairItems)
		{
		      RSSVRepairItem origItem = SelectFrom<RSSVRepairItem>.
                      Where<RSSVRepairItem.inventoryID.IsEqual<@P.AsInt>>.View.
                      Select(this, item.InventoryID);
                    if (origItem != null)
                    {
                        item.BasePrice = origItem.BasePrice;
                        RepairItems.Update(item);
                    }
		}

		Actions.PressSave();
	}
}

protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
{
	...
	
	UpdateItemPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
}