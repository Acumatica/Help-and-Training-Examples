using PX.Data;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class InventoryItemMaint_Extension : PXGraphExtension<PX.Objects.IN.InventoryItemMaint>
    {
        #region Event Handlers
        protected void _(Events.RowSelected<InventoryItem> e)
        {
            if (e.Row == null) return;
            InventoryItem item = e.Row;
            InventoryItemExt itemExt = PXCache<InventoryItem>.
                GetExtension<InventoryItemExt>(item);
            bool enableFields = itemExt != null &&
                itemExt.UsrRepairItem == true;
            //Make the Repair Item Type box available
            //when the Repair Item check box is selected.
            PXUIFieldAttribute.SetEnabled<InventoryItemExt.usrRepairItemType>(
                e.Cache, e.Row, enableFields);
        }

        #endregion
    }
}