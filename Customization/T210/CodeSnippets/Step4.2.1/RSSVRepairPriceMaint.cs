using System;
using System.Linq;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint :
        PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {
        #region Data Views
        public SelectFrom<RSSVRepairPrice>.View RepairPrices;

        public SelectFrom<RSSVRepairItem>.
            Where<RSSVRepairItem.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>.
            And<RSSVRepairItem.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>>>.View
            RepairItems;

        public SelectFrom<RSSVLabor>.
            Where<RSSVLabor.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVLabor.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.View
            Labor;
        ////////// The added code
        public SelectFrom<RSSVWarranty>.
            Where<RSSVWarranty.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVWarranty.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
            OrderBy<RSSVWarranty.defaultWarranty.Desc>.View
            Warranty;
        ////////// The end of added code
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
                InventoryItem item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                e.Cache.SetValueExt<RSSVRepairItem.repairItemType>(
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
                InventoryItem item = PXSelectorAttribute.
                    Select<RSSVRepairItem.inventoryID>(e.Cache, row)
                    as InventoryItem;
                //Retrieve the base price for the stock item.
                InventoryItemCurySettings curySettings =
                    InventoryItemCurySettings.PK.Find(
                    this, item.InventoryID, Accessinfo.BaseCuryID ?? "USD");
                //Copy the base price from the stock item to the row.
                e.NewValue = curySettings.BasePrice;
            }
        }

        //Update the IsDefault and Required fields of other records 
        //with the same repair item type when these fields are updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault,
                                     RSSVRepairItem.required>(e.Row, e.OldRow)) return;

            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select()
                .Where(item => item.GetItem<RSSVRepairItem>()
                .RepairItemType == row.RepairItemType);
            foreach (RSSVRepairItem repairItem in repairItems)
            {
                if (repairItem.LineNbr == row.LineNbr) continue;

                //Set IsDefault to false for all other items.
                if (row.IsDefault == true && repairItem.IsDefault == true)
                {
                    repairItem.IsDefault = false;
                    RepairItems.Update(repairItem);
                }
                //Make the Required field identical for all items of the type.
                if (row.Required != e.OldRow.Required &&
                    repairItem.Required != row.Required)
                {
                    repairItem.Required = row.Required;
                    RepairItems.Update(repairItem);
                }
            }
            //Refresh the UI.
            RepairItems.View.RequestRefresh();
        }

        //When Repair Item Type is updated,
        //clear the values of the Inventory ID and Default columns and
        //trigger FieldDefaulting for the Required column.
        protected void _(Events.FieldUpdated<RSSVRepairItem,
            RSSVRepairItem.repairItemType> e)
        {
            RSSVRepairItem row = e.Row;
            e.Cache.SetDefaultExt<RSSVRepairItem.required>(row);
            if (e.OldValue != null)
            {
                e.Cache.SetValueExt<RSSVRepairItem.inventoryID>(row, null);
                e.Cache.SetValue<RSSVRepairItem.isDefault>(row, false);
            }
        }

        //Set the value of the Required column.
        protected void _(Events.FieldDefaulting<RSSVRepairItem,
            RSSVRepairItem.required> e)
        {
            RSSVRepairItem row = e.Row;
            if (row.RepairItemType != null)
            {
                // Use LINQ to check whether there are any repair items 
                // with the same repair item type.
                var repairItem = (RSSVRepairItem)RepairItems.Select()
                    .FirstOrDefault(item =>
                        item.GetItem<RSSVRepairItem>().RepairItemType ==
                            row.RepairItemType &&
                        item.GetItem<RSSVRepairItem>().LineNbr != row.LineNbr);
                //Copy the Required value from the previous records.
                e.NewValue = repairItem?.Required;
            }
        }
        #endregion
    }
}