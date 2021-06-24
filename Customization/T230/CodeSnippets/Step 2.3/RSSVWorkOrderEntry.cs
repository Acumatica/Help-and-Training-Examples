public PXAction<RSSVWorkOrder> UpdateLaborPrices;
[PXButton(CommitChanges = true)]
[PXUIField(DisplayName = "Update Prices", Enabled = true)]
protected virtual void updateLaborPrices()
{
	var order = WorkOrders.Current;
	if (order.ServiceID != null && order.DeviceID != null)
	{
		var laborItems = Labor.Select();
		foreach (RSSVWorkOrderLabor labor in laborItems)
		{
			RSSVLabor origItem = SelectFrom<RSSVLabor>.
				Where<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>.View.
				Select(this, labor.InventoryID);
			if (origItem != null)
			{
				labor.DefaultPrice = origItem.DefaultPrice;
				Labor.Update(labor);
			}
		}

		Actions.PressSave();
	}
}