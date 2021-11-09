using PX.Objects.IN;
...

//Update price and repair item type when inventory ID of repair item is updated.
protected void _(Events.FieldUpdated<RSSVWorkOrderItem, RSSVWorkOrderItem.inventoryID> e)
{
	RSSVWorkOrderItem row = e.Row;
	if (row.InventoryID != null && row.RepairItemType == null)
	{
		//Use the PXSelector attribute to select the stock item.
		InventoryItem item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
		//Copy the repair item type from the stock item to the row.
		InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
		row.RepairItemType = itemExt.UsrRepairItemType;
	}
	e.Cache.SetDefaultExt<RSSVWorkOrderItem.basePrice>(e.Row);
}

protected void _(Events.FieldDefaulting<RSSVWorkOrderItem, RSSVWorkOrderItem.basePrice> e)
{
	RSSVWorkOrderItem row = e.Row;
	if (row.InventoryID != null)
	{
		//Use the PXSelector attribute to select the stock item.
		InventoryItem item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(
			e.Cache, row) as InventoryItem;
		//Retrieve the base price for the stock item.
		InventoryItemCurySettings curySettings = InventoryItemCurySettings.PK.Find(
			this, item.InventoryID, Accessinfo.BaseCuryID ?? "USD");
		//Copy the base price from the stock item to the row.
		e.NewValue = curySettings.BasePrice;
	}
}