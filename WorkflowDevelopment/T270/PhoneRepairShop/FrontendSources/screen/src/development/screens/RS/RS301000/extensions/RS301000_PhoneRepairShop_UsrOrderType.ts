import {
  PXFieldState,
  fieldInfo
} from "client-controls";
import { RS301000, RSSVWorkOrder} from "src/customizationScreens/Company/screens/RS/RS301000/RS301000";

export interface RS301000_PhoneRepairShop_UsrOrderType extends RS301000 {}
export class RS301000_PhoneRepairShop_UsrOrderType {}

export interface RSSVWorkOrder_PhoneRepairShop_UsrOrderType
  extends RSSVWorkOrder {}
export class RSSVWorkOrder_PhoneRepairShop_UsrOrderType {
  @fieldInfo({ commitChanges: true })
  UsrOrderType: PXFieldState;
}
