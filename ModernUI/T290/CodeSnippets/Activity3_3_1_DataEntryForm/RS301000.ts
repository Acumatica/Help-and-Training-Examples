import {
	PXScreen, createCollection, graphInfo,
	viewInfo, createSingle,
	PXView, PXFieldOptions, PXFieldState, controlConfig,
	gridConfig, GridPreset
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVWorkOrderEntry",
	primaryView: "WorkOrders"
})
export class RS301000 extends PXScreen {
	@viewInfo({ containerName: "Work Order" })
	WorkOrders = createSingle(RSSVWorkOrder);
	
	@viewInfo({ containerName: "Repair Items" })
	RepairItems = createCollection(RSSVWorkOrderItem);
	
	@viewInfo({ containerName: "Labor" })
	Labor = createCollection(RSSVWorkOrderLabor);
}

export class RSSVWorkOrder extends PXView {
	OrderNbr: PXFieldState;
	
	@controlConfig({allowEdit: true, })
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	DateCreated: PXFieldState;
	DateCompleted: PXFieldState;
	Status: PXFieldState;
	
	@controlConfig({rows: 2})
	Description : PXFieldState<PXFieldOptions.Multiline>;
	
	@controlConfig({allowEdit: true, })
	ServiceID : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@controlConfig({allowEdit: true, })
	DeviceID: PXFieldState<PXFieldOptions.CommitChanges>;
	OrderTotal: PXFieldState;
	Assignee: PXFieldState;
	Priority: PXFieldState<PXFieldOptions.CommitChanges>;
	InvoiceNbr: PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
	RepairItemType: PXFieldState;
	InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description: PXFieldState;
	Price: PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVWorkOrderLabor extends PXView {
	InventoryID: PXFieldState;
	InventoryID_description: PXFieldState;
	DefaultPrice: PXFieldState;
	Quantity: PXFieldState<PXFieldOptions.CommitChanges>;
	ExtPrice: PXFieldState;
}