import {
	PXScreen, graphInfo, createCollection,
} from "client-controls";

@graphInfo({
	graphType: "PhoneRepairShop.RSSVRepairServiceMaint",
	primaryView: "RepairService",
	hideFilesIndicator: true,
	hideNotesIndicator: true,
})
export class RS201000 extends PXScreen {
	RepairService = createCollection(RSSVRepairService);
}