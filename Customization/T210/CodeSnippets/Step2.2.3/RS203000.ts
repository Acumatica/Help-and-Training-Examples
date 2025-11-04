
import { createCollection, createSingle, graphInfo, PXView, PXScreen, PXFieldState, gridConfig, PXFieldOptions, PXActionState, GridPreset } from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairPriceMaint",
	primaryView: "RepairPrices",
})
export class RS203000 extends PXScreen {
	RepairPrices = createSingle(RSSVRepairPrice);
	RepairItems = createCollection(RSSVRepairItem);
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
	Required : PXFieldState;
	InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description : PXFieldState;
	BasePrice : PXFieldState;
	IsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
}