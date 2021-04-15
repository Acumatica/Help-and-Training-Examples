using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Linq;
using PX.Objects.CT;

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
        #endregion

        #region Event Handlers
        ...

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
                        item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType &&
                        item.GetItem<RSSVRepairItem>().LineNbr != row.LineNbr);
                //Copy the Required value from the previous records.
                e.NewValue = repairItem?.Required;
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
        #endregion

    }
}