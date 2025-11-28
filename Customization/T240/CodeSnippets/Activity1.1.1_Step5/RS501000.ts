import {
	PXScreen, createCollection, graphInfo,
	viewInfo,
	PXView, PXFieldOptions, PXFieldState, 
	gridConfig, columnConfig, GridPreset
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVAssignProcess",
	primaryView: "WorkOrders"
})
export class RS501000 extends PXScreen {
	@viewInfo({containerName: "Work Orders to Assign"})
	WorkOrders = createCollection(RSSVWorkOrder);
}

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
	Assignee: PXFieldState;
}