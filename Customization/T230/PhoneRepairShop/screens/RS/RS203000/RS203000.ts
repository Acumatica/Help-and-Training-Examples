import { createCollection, createSingle, PXScreen, 
	graphInfo, PXActionState, viewInfo,
	PXView, PXFieldState, gridConfig,
	PXFieldOptions, columnConfig,
	GridPreset, GridFastFilterVisibility } from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairPriceMaint", 
	primaryView: "RepairPrices", })
export class RS203000 extends PXScreen {

	RepairPrices = createSingle(RSSVRepairPrice);
   	@viewInfo({containerName: "Repair Items"})
	RepairItems = createCollection(RSSVRepairItem);
   	@viewInfo({containerName: "Labor"})
	Labor = createCollection(RSSVLabor);
   	@viewInfo({containerName: "Warranty"})
	Warranty = createCollection(RSSVWarranty);
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
	IsPriceValidated: PXFieldState;
	IsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
	ValidateItemPrices: PXActionState;
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

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class RSSVWarranty extends PXView  {
	ContractID : PXFieldState<PXFieldOptions.CommitChanges>;
	ContractID_description : PXFieldState;
	ContractDuration : PXFieldState;
	ContractDurationType : PXFieldState;
	ContractType : PXFieldState;
}