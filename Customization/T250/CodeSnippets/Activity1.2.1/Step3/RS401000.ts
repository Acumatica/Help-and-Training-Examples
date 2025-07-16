import {
    ////////// The added code
	createSingle, PXFieldOptions,
	////////// The end of added code
	createCollection, PXScreen, graphInfo, viewInfo,
	PXView, PXFieldState, gridConfig, GridPreset
} from "client-controls";

////////// The modified code
@graphInfo({
	graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
	primaryView: "Filter",
})
////////// The modified code
export class RS401000 extends PXScreen {
    ////////// The added code
	@viewInfo({ containerName: "Selection Area" })
	Filter = createSingle(RSSVWorkOrderToPayFilter);
	////////// The end of added code

	@viewInfo({ containerName: "Work Orders with Open Payments" })
	DetailsView = createCollection(RSSVWorkOrderToPay);
}

////////// The added code
export class RSSVWorkOrderToPayFilter extends PXView {
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
}
////////// The end of added code

@gridConfig({
	preset: GridPreset.Inquiry
})
export class RSSVWorkOrderToPay extends PXView {
	OrderNbr: PXFieldState;
	Status: PXFieldState;
	InvoiceNbr: PXFieldState;
	PercentPaid: PXFieldState;
	ARInvoice__DueDate: PXFieldState;
	ARInvoice__CuryDocBal: PXFieldState;
}