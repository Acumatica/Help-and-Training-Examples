using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Linq;
using PX.Objects.CT;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint : PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
  {
        #region Data Views
        //The primary view
        public SelectFrom<RSSVRepairPrice>.View RepairPrices;

        //The view for the Repair Items tab
        public SelectFrom<RSSVRepairItem>.
            LeftJoin<InventoryItem>.
                On<InventoryItem.inventoryID.IsEqual<
                RSSVRepairItem.inventoryID.FromCurrent>>.
            Where<RSSVRepairItem.deviceID.IsEqual<
                RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVRepairItem.serviceID.IsEqual<
                RSSVRepairPrice.serviceID.FromCurrent>>>.
            View RepairItems;

        //The view for the Labor tab
        public SelectFrom<RSSVLabor>.
            LeftJoin<InventoryItem>.
                On<InventoryItem.inventoryID.IsEqual<RSSVLabor.inventoryID.FromCurrent>>.
            Where<RSSVLabor.deviceID.IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
                And<RSSVLabor.serviceID.IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
            View Labor;

        //The view for the Warranty tab
        public SelectFrom<RSSVWarranty>.
            LeftJoin<ContractTemplate>.
                On<ContractTemplate.contractID.IsEqual<
                    RSSVWarranty.contractID.FromCurrent>>.
            Where<RSSVWarranty.deviceID.IsEqual<
                RSSVRepairPrice.deviceID.FromCurrent>.
                And<RSSVWarranty.serviceID.IsEqual<
                RSSVRepairPrice.serviceID.FromCurrent>>>.
            OrderBy<RSSVWarranty.defaultWarranty.Desc>.
            View Warranty;

        //The view for the default warranty
        public SelectFrom<Contract>.
            Where<Contract.contractCD.IsEqual<defaultWarranty>>.
            View DefaultWarranty;
        #endregion

        #region Event Handlers
        //Event handlers

		//Update the IsDefault or Required field of other records with the same repair item type when the IsDefault or Required field is updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items with the same repair item type
            //as in the updated row.
            var repairItems = RepairItems.Select().Where(item =>
            item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType);
            foreach (RSSVRepairItem repairItem in repairItems)
            {
                if (repairItem.LineNbr != row.LineNbr)
                {
                    //Set IsDefault to false for all other items.
                    if (row.IsDefault == true)
                    {
                        repairItem.IsDefault = false;
                        RepairItems.Update(repairItem);
                    }
                    //Make the Required field identical for all items.
                    if (row.Required != e.OldRow.Required && row.Required != repairItem.Required)
                    {
                        repairItem.Required = row.Required;
                        RepairItems.Update(repairItem);
                    }
                }
            }

            //Refresh the UI.
            RepairItems.View.RequestRefresh();
        }

        //Update the Required check box when a repair item type is selected.
        protected void _(Events.FieldUpdated<RSSVRepairItem,
        RSSVRepairItem.repairItemType> e)
        {
            RSSVRepairItem row = e.Row;
            //Use LINQ to check whether there are any repair items with the same
            //repair item type.
            var repairItem = (RSSVRepairItem)RepairItems.Select().Where(item =>
            item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType).
            FirstOrDefault();
            //Copy the Required value from the previous records.
            if (repairItem != null) row.Required = repairItem.Required;
        }

        //Insert the default detail record.
        protected virtual void _(Events.RowInserted<RSSVRepairPrice> e)
        {
            if (Warranty.Select().Count == 0)
            {
                bool oldDirty = Warranty.Cache.IsDirty;
                // Retrieve the default warranty.
                Contract defaultWarranty = (Contract)DefaultWarranty.Select();
                if (defaultWarranty != null)
                {
                    RSSVWarranty line = new RSSVWarranty();
                    line.ContractID = defaultWarranty.ContractID;
                    // Insert the data record into
                    // the cache of the Warranty data view.
                    Warranty.Insert(line);
                    // Clear the flag that indicates in the UI whether the cache
                    // contains changes.
                    Warranty.Cache.IsDirty = oldDirty;
                }
            }
        }

        //Set the DefaultWarranty field to true for the inserted default warranty
        protected virtual void _(Events.FieldDefaulting<RSSVWarranty.defaultWarranty> e)
        {
            RSSVWarranty line = (RSSVWarranty)e.Row;
            if (line == null) return;
            Contract defaultWarranty = (Contract)DefaultWarranty.Select();
            if (defaultWarranty != null && line.ContractID ==
                defaultWarranty.ContractID)
            {
                //Setting the default value
                e.NewValue = true;
                // Setting a flag to prevent the execution of the FieldDefaulting event
                // handlers that are defined in attributes
                e.Cancel = true;
            }
        }

        //Make the default warranty unavailable for editing.
        protected virtual void _(Events.RowSelected<RSSVWarranty> e)
        {
            RSSVWarranty line = e.Row;
            if (line == null) return;
            PXUIFieldAttribute.SetEnabled(e.Cache, line, line.DefaultWarranty != true);
        }
        #endregion

        #region Supplementary Classes
        //The FBQL constant for the free warranty that is inserted by default
        public const string DefaultWarrantyConstant = "DFLTWARRNT";
        public class defaultWarranty : PX.Data.BQL.BqlString.Constant<defaultWarranty>
        {
            public defaultWarranty()
            : base(DefaultWarrantyConstant)
            {
            }
        }
        #endregion
    }
}