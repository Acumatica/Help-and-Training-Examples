import {
	PXScreen, createCollection, graphInfo,
	viewInfo
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVAssignProcess",
	primaryView: "WorkOrders"
})
export class RS501000 extends PXScreen {
	@viewInfo({containerName: "Work Orders to Assign"})
	WorkOrders = createCollection(RSSVWorkOrder);
}