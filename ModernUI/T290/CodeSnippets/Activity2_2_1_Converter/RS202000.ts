import { 
	createSingle, PXScreen, graphInfo, PXView, PXFieldState
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVDeviceMaint",
	primaryView: "ServDevices",
})
export class RS202000 extends PXScreen {
	ServDevices = createSingle(RSSVDevice);
}

// View
export class RSSVDevice extends PXView  {
	DeviceCD : PXFieldState;
	Description : PXFieldState;
	Active : PXFieldState;
	AvgComplexityOfRepair : PXFieldState;
}