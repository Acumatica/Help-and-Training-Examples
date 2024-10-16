import {
	IN202500,
	ItemSettings
} from "src/screens/IN/IN202500/IN202500";
import {
	PXFieldState,
	PXFieldOptions,
	createCollection,
	PXView,
	gridConfig,
	GridPreset,
} from "client-controls";

export interface IN202500_PhoneRepairShop extends IN202500 { }
export class IN202500_PhoneRepairShop {
    CompatibleDevices = createCollection(RSSVStockItemDevice);
}

export interface ItemSettings_PhoneRepairShop extends ItemSettings { }
export class ItemSettings_PhoneRepairShop {
	UsrRepairItem: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrRepairItemType: PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVStockItemDevice extends PXView {
	DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;
	DeviceID_description: PXFieldState;
}