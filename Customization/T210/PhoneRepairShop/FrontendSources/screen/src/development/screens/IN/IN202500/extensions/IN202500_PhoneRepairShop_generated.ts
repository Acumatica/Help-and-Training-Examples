import {
  PXFieldState,
  PXView,
  viewInfo,
  gridConfig,
  fieldInfo,
  GridPreset,
  createCollection,
  PXFieldOptions
} from "client-controls";
import { IN202500, ItemSettings } from "src/screens/IN/IN202500/IN202500";

export interface IN202500_PhoneRepairShop_generated extends IN202500 {}
export class IN202500_PhoneRepairShop_generated {
  @viewInfo({ containerName: "Compatible Devices" })
  CompatibleDevices = createCollection(RSSVStockItemDevice);
}

export interface ItemSettings_PhoneRepairShop_generated extends ItemSettings {}
export class ItemSettings_PhoneRepairShop_generated {
  @fieldInfo({ commitChanges: true })
  UsrRepairItem: PXFieldState;

  UsrRepairItemType: PXFieldState;
}

@gridConfig({ preset: GridPreset.Details })
export class RSSVStockItemDevice extends PXView {
  DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;
  DeviceID_description: PXFieldState;
}
