import {
	PXScreen, createCollection, graphInfo,
	viewInfo, 
	////////// The added code
	createSingle,
	////////// The end of added code
	PXView, PXFieldOptions, PXFieldState,
	gridConfig, columnConfig, GridPreset
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVAssignProcess",
	////////// The modified code
	primaryView: "Filter"
	////////// The end of modified code
})
export class RS501000 extends PXScreen {
	////////// The added code
	@viewInfo({containerName: "Filter Parameters"})
	Filter = createSingle(RSSVWorkOrderToAssignFilter);
	////////// The end of added code
	
	@viewInfo({containerName: "Work Orders to Assign"})
	WorkOrders = createCollection(RSSVWorkOrder);
}

////////// The added code
export class RSSVWorkOrderToAssignFilter extends PXView {
	Priority: PXFieldState<PXFieldOptions.CommitChanges>;
	TimeWithoutAction: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
}
////////// The end of added code

@gridConfig({
	preset: GridPreset.Processing,
	autoAdjustColumns: true
})
export class RSSVWorkOrder extends PXView { 
	@columnConfig({ allowCheckAll: true })
	Selected: PXFieldState; 

	@columnConfig({ hideViewLink: true })
	OrderNbr: PXFieldState;
	Description: PXFieldState;

	@columnConfig({ hideViewLink: true })
	ServiceID: PXFieldState;
	
	@columnConfig({ hideViewLink: true })
	DeviceID: PXFieldState;

	Priority: PXFieldState;
	
	@columnConfig({ hideViewLink: true})
	////////// The modified code
	Assignee: PXFieldState<PXFieldOptions.CommitChanges>;
	////////// The end of modified code
    ////////// The added code
	TimeWithoutAction: PXFieldState;
	////////// The end of added code
}