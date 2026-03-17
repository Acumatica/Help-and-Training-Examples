import {
	createSingle, PXFieldOptions,
	createCollection, PXScreen, graphInfo, viewInfo,
	PXView, PXFieldState, gridConfig, GridPreset,
        ////////// The added code
        PXPageLoadBehavior
        ////////// The end of added code
} from "client-controls";

////////// The modified code
@graphInfo({
	graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
	primaryView: "Filter",
	pageLoadBehavior: PXPageLoadBehavior.PopulateSavedValues,
})
////////// The end of modified code
export class RS401000 extends PXScreen {
	@viewInfo({ containerName: "Selection Area" })
	Filter = createSingle(RSSVWorkOrderToPayFilter);

	@viewInfo({ containerName: "Work Orders with Open Payments" })
	DetailsView = createCollection(RSSVWorkOrderToPay);
}

export class RSSVWorkOrderToPayFilter extends PXView {
	CustomerID: PXFieldState<PXFieldOptions.CommitChanges>;
	ServiceID: PXFieldState<PXFieldOptions.CommitChanges>;
}

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