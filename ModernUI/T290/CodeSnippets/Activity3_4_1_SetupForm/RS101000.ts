import {
	PXScreen,
	createSingle,
	graphInfo,
	PXView,
	PXFieldState,	
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVSetupMaint",
	primaryView: "Setup",
})
export class RS101000 extends PXScreen {
	Setup = createSingle(RSSVSetup);
}

export class RSSVSetup extends PXView {
	NumberingID: PXFieldState;
	WalkInCustomerID: PXFieldState;
	DefaultEmployee: PXFieldState;
	PrepaymentPercent: PXFieldState;
}