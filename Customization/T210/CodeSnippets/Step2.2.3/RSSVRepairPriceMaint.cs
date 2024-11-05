using System;
////////// The added code
using System.Linq;
////////// The end of added code
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint :
        PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {
        #region Data Views
        public SelectFrom<RSSVRepairPrice>.View RepairPrices = null!;

        public SelectFrom<RSSVRepairItem>.
            Where<RSSVRepairItem.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>.
            And<RSSVRepairItem.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>>>.View
            RepairItems = null!;
        #endregion

        #region Event Handlers
        //Update the price and repair item type when the inventory ID of
        //the repair item is updated.
        protected void _(Events.FieldUpdated<RSSVRepairItem,
            RSSVRepairItem.inventoryID> e)
        {
            RSSVRepairItem row = e.Row;

            if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                var item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                var itemExt = item?.GetExtension<InventoryItemExt>();
                if (itemExt != null) e.Cache.SetValueExt<RSSVRepairItem.repairItemType>(
                    row, itemExt.UsrRepairItemType);
            }
            //Trigger the FieldDefaulting event handler for basePrice.
            e.Cache.SetDefaultExt<RSSVRepairItem.basePrice>(e.Row);
        }

        //Set the value of the Price column.
        protected void _(Events.FieldDefaulting<RSSVRepairItem,
            RSSVRepairItem.basePrice> e)
        {
            RSSVRepairItem row = e.Row;
            if (row.InventoryID != null)
            {
                //Use the PXSelector attribute to select the stock item.
                var item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Retrieve the base price for the stock item.
                var curySettings =
                    InventoryItemCurySettings.PK.Find(
                    this, item?.InventoryID, Accessinfo.BaseCuryID ?? "USD");
                //Copy the base price from the stock item to the row.
                if (curySettings != null) e.NewValue = curySettings.BasePrice;
            }
        }

        ////////// The added code
        //Update the IsDefault field of other records with the same repair item type 
        //when the IsDefault field is updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            // Make sure the handler runs only when the IsDefault field is edited.
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault>(e.Row, e.OldRow))
                return;

            RSSVRepairItem row = e.Row;

            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select().Where(item =>
                item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType);

            foreach (RSSVRepairItem repairItem in repairItems)
            {
                if (repairItem.LineNbr == row.LineNbr) continue;

                //Set IsDefault to false for all other items.
                if (row.IsDefault == true && repairItem.IsDefault == true)
                {
                    repairItem.IsDefault = false;
                    RepairItems.Update(repairItem);
                }
            }

            //Refresh the UI.		
            RepairItems.View.RequestRefresh();
        }
        ////////// The end of added code
        #endregion
    }
}