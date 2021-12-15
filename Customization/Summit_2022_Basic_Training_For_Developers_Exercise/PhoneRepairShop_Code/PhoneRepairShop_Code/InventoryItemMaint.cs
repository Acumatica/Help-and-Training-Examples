using PX.Data;
using PhoneRepairShop;
using PX.Data.BQL.Fluent;

namespace PX.Objects.IN
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class InventoryItemMaint_Extension : PXGraphExtension<InventoryItemMaint>
  {
        #region Data Views
        public SelectFrom<RSSVStockItemDevice>.
            Where<RSSVStockItemDevice.inventoryID.
                IsEqual<InventoryItem.inventoryID.FromCurrent>>.View
            CompatibleDevices;
        #endregion

        #region Event Handlers
        protected void _(Events.RowSelected<InventoryItem> e)
        {
            InventoryItem item = e.Row;
            InventoryItemExt itemExt = PXCache<InventoryItem>.
                GetExtension<InventoryItemExt>(item);
            bool enableFields = itemExt != null &&
                itemExt.UsrRepairItem == true;
            //Make the Repair Item Type box available
            //when the Repair Item check box is selected.
            PXUIFieldAttribute.SetEnabled<InventoryItemExt.usrRepairItemType>(
                e.Cache, e.Row, enableFields);

            //Display the Compatible Devices tab when the Repair Item check box 
            //is selected.
            CompatibleDevices.Cache.AllowSelect = enableFields;
        }
        #endregion
    }
}