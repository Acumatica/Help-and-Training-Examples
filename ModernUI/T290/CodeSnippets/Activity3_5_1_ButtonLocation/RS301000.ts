import {
	PXScreen, createCollection, graphInfo,
	viewInfo, createSingle,
	PXView, PXFieldOptions, PXFieldState, controlConfig,
	gridConfig, GridPreset,
    fieldConfig,
    PXActionState
} from "client-controls";

@graphInfo({
    graphType: "PhoneRepairShop.RSSVWorkOrderEntry",
    primaryView: "WorkOrders"
})
export class RS301000 extends PXScreen {
  OpenInvoice: PXActionState;

  WorkOrders = createSingle(RSSVWorkOrder);
  RepairItems = createCollection(RSSVWorkOrderItem);
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
    @fieldConfig({
        pinned: true
    })
	Priority: PXFieldState<PXFieldOptions.CommitChanges>;
	InvoiceNbr: PXFieldState;
}

@gridConfig({
	preset: GridPreset.Details
})
export class RSSVWorkOrderItem extends PXView {
    PressMe : PXActionState;
	RepairItemType: PXFieldState;
	InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID_description: PXFieldState;
	BasePrice: PXFieldState;
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