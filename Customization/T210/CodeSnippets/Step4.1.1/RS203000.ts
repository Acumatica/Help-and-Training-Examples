
import { createCollection, createSingle, graphInfo, PXView, PXScreen, PXFieldState, gridConfig, PXFieldOptions, PXActionState, GridPreset, viewInfo } from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairPriceMaint",
	primaryView: "RepairPrices",
})
export class RS203000 extends PXScreen {
	@viewInfo({containerName: "Repair Prices"})
	RepairPrices = createSingle(RSSVRepairPrice);
	@viewInfo({containerName: "Repair Items"})
	RepairItems = createCollection(RSSVRepairItem);
	@viewInfo({containerName: "Labor"})
	Labor = createCollection(RSSVLabor);
}

export class RSSVRepairPrice extends PXView {
  ServiceID: PXFieldState;
  DeviceID: PXFieldState;
  Price: PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details,
	syncPosition: true,
	initNewRow: true
})
export class RSSVRepairItem extends PXView {
    RepairItemType : PXFieldState<PXFieldOptions.CommitChanges>;
	Required : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description : PXFieldState;
	BasePrice : PXFieldState;
	IsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVLabor extends PXView  {
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description : PXFieldState;
	DefaultPrice : PXFieldState;
	Quantity : PXFieldState;
	ExtPrice : PXFieldState;
}