import {
	createSingle, PXFieldOptions, PXPageLoadBehavior,
	createCollection, PXScreen, graphInfo, viewInfo,
	PXView, PXFieldState, gridConfig, GridPreset,
	////////// The added code
	linkCommand, PXActionState
	////////// The end of added code
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
	primaryView: "Filter",
	pageLoadBehavior: PXPageLoadBehavior.PopulateSavedValues,
})
export class RS401000 extends PXScreen {
    ////////// The added code
	ViewOrder: PXActionState;
	////////// The end of added code

	@viewInfo({ containerName: "Selection Area" })
	Filter = createSingle(RSSVWorkOrderToPayFilter);

	@viewInfo({ containerName: "Work Orders with Open Payments" })
	DetailsView = createCollection(RSSVWorkOrderToPay);
}

export class RSSVWorkOrderToPayFilter extends PXView {
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
	GroupByStatus: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
	OrderType: PXFieldState;
    ////////// The added code
	@linkCommand<RS401000>("ViewOrder")
	OrderNbr: PXFieldState;
	////////// The end of added code
	Status: PXFieldState;
	InvoiceNbr: PXFieldState<PXFieldOptions.CommitChanges>;
	PercentPaid: PXFieldState;
	ARInvoice__DueDate: PXFieldState;
	ARInvoice__CuryDocBal: PXFieldState;
}