import {
	PXScreen,
	createSingle,
	graphInfo,
	PXView,
	PXFieldState,
	controlConfig,
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVSetupMaint",
	primaryView: "Setup",
})
export class RS101000 extends PXScreen {
	Setup = createSingle(RSSVSetup);
}

export class RSSVSetup extends PXView {
	@controlConfig({allowEdit: true, })
	NumberingID: PXFieldState;
	@controlConfig({allowEdit: true, })
	WalkInCustomerID: PXFieldState;
	DefaultEmployee: PXFieldState;
	PrepaymentPercent: PXFieldState;
}