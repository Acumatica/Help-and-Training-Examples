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

export class RSSVWorkOrderToPayFilter extends PXView {
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
	GroupByStatus: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Inquiry
})
////////// The modified code
export class RSSVWorkOrderToPay extends PXView {
	OrderType: PXFieldState;
	OrderNbr: PXFieldState;
	Status: PXFieldState;
	InvoiceNbr: PXFieldState<PXFieldOptions.CommitChanges>;
	PercentPaid: PXFieldState;
	ARInvoice__DueDate: PXFieldState;
	ARInvoice__CuryDocBal: PXFieldState;
}
////////// The end of added code