using System;
using PX.Data;
using PX.Objects.IN;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {
	...
        #region Event Handlers
        ...
        //Update Repair Item Type and Price according to Inventory ID
        protected virtual void _(Events.FieldUpdated<RSSVWorkOrderItem, RSSVWorkOrderItem.inventoryID> e)
        { 
            RSSVWorkOrderItem row = e.Row;
        if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                InventoryItem item = PXSelectorAttribute.
                Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                row.RepairItemType = itemExt.UsrRepairItemType;
             }
             e.Cache.SetDefaultExt<RSSVWorkOrderItem.basePrice>(e.Row);
        }

        protected void _(Events.FieldDefaulting<RSSVWorkOrderItem, RSSVWorkOrderItem.basePrice> e)
        {
            RSSVWorkOrderItem row = e.Row;
            if (row.InventoryID != null)
            {
                //Use the PXSelector attribute to select the stock item.
                InventoryItem item = PXSelectorAttribute.
                Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the base price from the stock item to the row.
                e.NewValue = item.BasePrice;
            }
        }

        #endregion
    }
}