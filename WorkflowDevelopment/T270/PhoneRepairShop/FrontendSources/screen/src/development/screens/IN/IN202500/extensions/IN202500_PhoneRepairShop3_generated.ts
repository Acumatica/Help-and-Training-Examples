import {
  PXFieldState,
  PXFieldOptions,
  PXView,
  updateDecorator,
  removeDecorator,
  featureInstalled,
  FeaturesSet,
  placeBeforeView,
  placeAfterView,
  placeBeforeProperty,
  placeAfterProperty,
  graphInfo,
  viewInfo,
  gridConfig,
  columnConfig,
  treeConfig,
  controlConfig,
  linkCommand,
  fieldOptions,
  fieldConfig,
  fieldInfo,
  headerDescription,
  autoRefresh,
  mappedToViewField,
  suppressLabel,
  readOnly,
  disabled,
  unbound,
  text,
  multiLine,
  primaryKey,
  type,
  ControlParameter,
  NetType,
  MenuItemRenderType,
  ScrollMode,
  TextAlign,
  GridPreset,
  GridColumnType,
  GridColumnShowHideMode,
  GridColumnDisplayMode,
  GridAutoGrowMode,
  GridPagerMode,
  GridFastFilterVisibility,
  GridNoteFilesShowMode,
  HeaderDescription,
  GridColumnGeneration,
  GridFilterBarVisibility,
  NodeSelectMode,
  PXSelectorMode,
  ApplySuggestionMode,
  ITextEditorControlConfig,
  ITimeSpanConfig,
  ISelectorControlConfig,
  ISegmentedSelectorControlConfig,
  IEditorControlConfig,
  INumberEditorControlConfig,
  IMaskEditorControlConfig,
  IMailEditorControlConfig,
  ILinkEditorControlConfig,
  IDropDownConfig,
  IDatetimeEditControlConfig,
  IColorPickerControlConfig,
  ICheckBoxControlConfig,
  ICalendarControlConfig,
  IBranchSelectorConfig,
  IBarcodeInputControlConfig,
  ITreeSelectorConfig,
  IRichTextEditorConfig,
  IFormulaEditorConfig,
  ICurrencyControlConfig,
  FieldGenerationMode,
  createCollection
} from "client-controls";
import { IN202500, ItemSettings } from "src/screens/IN/IN202500/IN202500";

export interface IN202500_PhoneRepairShop3_generated extends IN202500 {}
export class IN202500_PhoneRepairShop3_generated {
  @gridConfig({ preset: GridPreset.Details })
  @viewInfo({ containerName: "Compatible Devices" })
  CompatibleDevices = createCollection(RSSVStockItemDevice);
}

export interface ItemSettings_PhoneRepairShop3_generated extends ItemSettings {}
export class ItemSettings_PhoneRepairShop3_generated {
  @fieldInfo({ commitChanges: true })
  UsrRepairItem: PXFieldState;

  UsrRepairItemType: PXFieldState;
}

export class RSSVStockItemDevice extends PXView {
  @fieldInfo({ commitChanges: true })
  DeviceID: PXFieldState;
  DeviceID_description: PXFieldState;
}
