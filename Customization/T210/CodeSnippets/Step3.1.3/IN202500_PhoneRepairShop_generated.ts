import {
  PXFieldState,
  PXView,
  viewInfo,
  gridConfig,
  fieldInfo,
  GridPreset,
  createCollection
} from "client-controls";
import { IN202500, ItemSettings } from "src/screens/IN/IN202500/IN202500";

export interface IN202500_PhoneRepairShop_generated extends IN202500 {}
export class IN202500_PhoneRepairShop_generated {
  @gridConfig({ preset: GridPreset.Details })
  @viewInfo({ containerName: "Compatible Devices" })
  CompatibleDevices = createCollection(RSSVStockItemDevice);
}

export interface ItemSettings_PhoneRepairShop_generated extends ItemSettings {}
export class ItemSettings_PhoneRepairShop_generated {
  @fieldInfo({ commitChanges: true })
  UsrRepairItem: PXFieldState;

  UsrRepairItemType: PXFieldState;
}

export class RSSVStockItemDevice extends PXView {
  @fieldInfo({ commitChanges: true })
  DeviceID: PXFieldState;
  DeviceID_description: PXFieldState;
}
