import {
	PXScreen, createCollection, graphInfo,
	viewInfo, createSingle,
	PXView, PXFieldOptions, PXFieldState,
	gridConfig, columnConfig, GridPreset
} from "client-controls";

@graphInfo({
	graphType : "PhoneRepairShop.RSSVAssignProcess",
	primaryView : "Filter"
})
export class RS501000 extends PXScreen {
	@viewInfo({containerName : "Filter Parameters"})
	Filter = createSingle(RSSVWorkOrderToAssignFilter);
	@viewInfo({containerName : "Work Orders to Assign"})
	WorkOrders = createCollection(RSSVWorkOrder);
}

export class RSSVWorkOrderToAssignFilter extends PXView {
	Priority : PXFieldState<PXFieldOptions.CommitChanges>;
	TimeWithoutAction : PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset : GridPreset.Inquiry,
	autoAdjustColumns : true
})
export class RSSVWorkOrder extends PXView { 
	@columnConfig({ allowCheckAll: true })
	Selected: PXFieldState; 

	@columnConfig({ hideViewLink: true })
	OrderNbr : PXFieldState;

	Description : PXFieldState;

	@columnConfig({ hideViewLink: true })
	ServiceID : PXFieldState;
	@columnConfig({ hideViewLink: true })
	DeviceID : PXFieldState;

	Priority : PXFieldState;
	@columnConfig({ hideViewLink: true})
	AssignTo : PXFieldState<PXFieldOptions.CommitChanges>;
	NbrOfAssignedOrders : PXFieldState;
	TimeWithoutAction : PXFieldState;
}