import { createCollection, createSingle, PXScreen, graphInfo, 
	viewInfo, PXView, PXFieldState, gridConfig, PXFieldOptions, 
	columnConfig, GridPreset,GridFastFilterVisibility } 
from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairPriceMaint", 
	primaryView: "RepairPrices", })
export class RS203000 extends PXScreen {
	@viewInfo({containerName: "Services and Prices"})
	RepairPrices = createSingle(RSSVRepairPrice);
   	@viewInfo({containerName: "Repair Items"})
	RepairItems = createCollection(RSSVRepairItem);
    @viewInfo({containerName: "Labor"})
	Labor = createCollection(RSSVLabor);
}// Views

export class RSSVRepairPrice extends PXView  {

	ServiceID : PXFieldState;
	DeviceID : PXFieldState;
	Price : PXFieldState;
}

@gridConfig({
	initNewRow: true,
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class RSSVRepairItem extends PXView  {
	RepairItemType : PXFieldState<PXFieldOptions.CommitChanges>;
	Required : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({hideViewLink: true})
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description : PXFieldState;
	BasePrice : PXFieldState<PXFieldOptions.CommitChanges>;
	IsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class RSSVLabor extends PXView  {
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description : PXFieldState;
	DefaultPrice : PXFieldState<PXFieldOptions.CommitChanges>;
	Quantity : PXFieldState<PXFieldOptions.CommitChanges>;
	ExtPrice : PXFieldState;
}
