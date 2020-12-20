using System;
using System.Linq;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CT;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint : PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {
		...


        #region Event handlers

		...

        //Update price and repair item type when inventory ID of repair item 
        //is updated.
        protected void _(Events.FieldUpdated<RSSVRepairItem, RSSVRepairItem.inventoryID> e)
        {
            RSSVRepairItem row = e.Row;

            if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                InventoryItem item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                row.RepairItemType = itemExt.UsrRepairItemType;
            }
            e.Cache.SetDefaultExt<RSSVRepairItem.basePrice>(e.Row);
        }
		
		protected void _(Events.FieldDefaulting<RSSVRepairItem, RSSVRepairItem.basePrice> e)
		{
			RSSVRepairItem row = e.Row;
			if (row.InventoryID != null)
			{
				//Use the PXSelector attribute to select the stock item.
				InventoryItem item = PXSelectorAttribute.
					Select<RSSVRepairItem.inventoryID>(e.Cache, row) as InventoryItem;
				//Copy the base price from the stock item to the row.
				e.NewValue = item.BasePrice;
			}
		}
		
		protected void _(Events.FieldUpdated<RSSVRepairItem, RSSVRepairItem.repairItemType> e)
		{
			RSSVRepairItem row = e.Row;
			if (e.OldValue != null)
			{
				e.Cache.SetValueExt<RSSVRepairItem.inventoryID>(row, null);
				e.Cache.SetValue<RSSVRepairItem.isDefault>(row, false);
			}
		}

        #endregion

		...
    }
}