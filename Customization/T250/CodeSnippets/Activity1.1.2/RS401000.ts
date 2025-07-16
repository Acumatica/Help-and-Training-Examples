import { createCollection, PXScreen, graphInfo, viewInfo,
	PXView, PXFieldState, gridConfig, PXFieldOptions, GridPreset
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVPaymentPlanInq",
	primaryView: "DetailsView",
})
export class RS401000 extends PXScreen {
    @viewInfo({containerName: "Work Orders with Open Payments"})
    DetailsView = createCollection(RSSVWorkOrderToPay);
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