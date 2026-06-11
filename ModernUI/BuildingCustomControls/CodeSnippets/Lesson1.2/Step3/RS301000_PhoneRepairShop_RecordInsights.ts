import { PXFieldState } from "client-controls";
import {
  RS301000,
  RSSVWorkOrder
} from "src/customizationScreens/Company/screens/RS/RS301000/RS301000";

export interface RS301000_PhoneRepairShop_RecordInsights extends RS301000 {}
export class RS301000_PhoneRepairShop_RecordInsights {}

export interface RSSVWorkOrder_PhoneRepairShop_RecordInsights
  extends RSSVWorkOrder {}
export class RSSVWorkOrder_PhoneRepairShop_RecordInsights {
  PanelStatus: PXFieldState;
  PanelWarningMessage: PXFieldState;
  PanelDetails: PXFieldState;
  PanelAccent: PXFieldState;
}