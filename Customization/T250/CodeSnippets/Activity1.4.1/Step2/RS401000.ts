import {
	createSingle, PXFieldOptions, PXPageLoadBehavior,
	createCollection, PXScreen, graphInfo, viewInfo,
	PXView, PXFieldState, gridConfig, GridPreset
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
	primaryView: "Filter",
	pageLoadBehavior: PXPageLoadBehavior.PopulateSavedValues,
})
export class RS401000 extends PXScreen {
	@viewInfo({ containerName: "Selection Area" })
	Filter = createSingle(RSSVWorkOrderToPayFilter);

	@viewInfo({ containerName: "Work Orders with Open Payments" })
	DetailsView = createCollection(RSSVWorkOrderToPay);
}

////////// The modified code
export class RSSVWorkOrderToPayFilter extends PXView {
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
	GroupByStatus: PXFieldState<PXFieldOptions.CommitChanges>;
}
////////// The end of modified code

@gridConfig({
	preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
	OrderType: PXFieldState;
	OrderNbr: PXFieldState;
	Status: PXFieldState;
	InvoiceNbr: PXFieldState;
	PercentPaid: PXFieldState;
	ARInvoice__DueDate: PXFieldState;
	ARInvoice__CuryDocBal: PXFieldState;
}