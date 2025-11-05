
import { createCollection, createSingle, graphInfo, PXView, PXScreen, PXFieldState, gridConfig, PXFieldOptions, PXActionState, GridPreset } from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairPriceMaint",
	primaryView: "RepairPrices",
})
export class RS203000 extends PXScreen {
	RepairPrices = createSingle(RSSVRepairPrice);
}

export class RSSVRepairPrice extends PXView {
  ServiceID: PXFieldState;
  DeviceID: PXFieldState;
  Price: PXFieldState;
}