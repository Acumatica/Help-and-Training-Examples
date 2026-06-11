import {
  CustomEventType,
  handleEvent,
  RowCssHandlerArgs,
} from "client-controls";

import {
  RS301000,
  RSSVWorkOrderItem
} from "src/customizationScreens/Company/screens/RS/RS301000/RS301000";

export interface RS301000_PhoneRepairShop_RepairItemsEvents
  extends RS301000 {}

export class RS301000_PhoneRepairShop_RepairItemsEvents {
  @handleEvent(CustomEventType.GetRowCss, { view: "RepairItems" as never})
   // "as never" works around a TypeScript inference issue with the decorator.
   // Runtime value is still "RepairItems".
    getRepairItemsRowCss(args: RowCssHandlerArgs<RSSVWorkOrderItem>): string {
        const item = args?.selector?.row;
        if (item != null && item.BasePrice.value > 15) {
            return "bold-row";
        }
        return undefined;
    }
}