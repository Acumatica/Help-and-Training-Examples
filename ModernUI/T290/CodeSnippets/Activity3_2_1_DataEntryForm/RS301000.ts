import {
  PXScreen, createCollection, graphInfo,
  viewInfo, createSingle,
  PXView, PXFieldOptions, PXFieldState,
  gridConfig, GridPreset
} from "client-controls";

@graphInfo({
    graphType : "PhoneRepairShop.RSSVWorkOrderEntry",
    primaryView : "WorkOrders"
})
export class RS301000 extends PXScreen {
  @viewInfo({ containerName: "Order Summary" })
  WorkOrders = createSingle(RSSVWorkOrder);
  RepairItems = createCollection(RSSVWorkOrderItem);
  Labor = createCollection(RSSVWorkOrderLabor);
}

export class RSSVWorkOrder extends PXView {
  OrderNbr : PXFieldState;
  CustomerID : PXFieldState<PXFieldOptions.CommitChanges>;
  DateCreated : PXFieldState;
  DateCompleted : PXFieldState;
  Status : PXFieldState;
  Description : PXFieldState;
  ServiceID : PXFieldState<PXFieldOptions.CommitChanges>;
  DeviceID : PXFieldState<PXFieldOptions.CommitChanges>;
  OrderTotal : PXFieldState;
  Assignee : PXFieldState;
  Priority : PXFieldState<PXFieldOptions.CommitChanges>;
  InvoiceNbr : PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
  RepairItemType : PXFieldState;
  InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
  InventoryID_description: PXFieldState;
  Price : PXFieldState;
}

@gridConfig({
    preset: GridPreset.Details
})
export class RSSVWorkOrderLabor extends PXView {
  InventoryID : PXFieldState;
  InventoryID_description: PXFieldState;
  DefaultPrice : PXFieldState;
  Quantity : PXFieldState<PXFieldOptions.CommitChanges>;
  ExtPrice : PXFieldState;
}