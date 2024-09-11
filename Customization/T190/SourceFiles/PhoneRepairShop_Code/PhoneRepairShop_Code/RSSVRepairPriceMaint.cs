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
        #region Views
        public SelectFrom<RSSVRepairPrice>.View RepairPrices = null!;

        public SelectFrom<RSSVRepairItem>.
            Where<RSSVRepairItem.serviceID.IsEqual<RSSVRepairPrice.serviceID.FromCurrent>.
                And<RSSVRepairItem.deviceID.IsEqual<RSSVRepairPrice.deviceID.FromCurrent>>>.View
                    RepairItems = null!;

        public SelectFrom<RSSVLabor>.
            Where<RSSVLabor.deviceID.IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
                And<RSSVLabor.serviceID.IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.View
                    Labor = null!;

        public SelectFrom<RSSVWarranty>.
            Where<RSSVWarranty.deviceID.IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
                And<RSSVWarranty.serviceID.IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
                OrderBy<RSSVWarranty.defaultWarranty.Desc>.View Warranty = null!;

        //The view for the default warranty
        public SelectFrom<ContractTemplate>.
            Where<ContractTemplate.contractCD.IsEqual<defaultWarranty>>.
            View DefaultWarranty = null!;
        #endregion


        #region Event handlers

        //Update the IsDefault field of other records with the same repair item type 
        //when the IsDefault field is updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault,
                                     RSSVRepairItem.required>(e.Row, e.OldRow)) return;

            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select()
                .Where(item => item.GetItem<RSSVRepairItem>().RepairItemType == row.RepairItemType);
            foreach (RSSVRepairItem repairItem in repairItems)
            {
                if (repairItem.LineNbr == row.LineNbr) continue;

                //Set IsDefault to false for all other items.
                if (row.IsDefault == true && repairItem.IsDefault == true)
                {
                    repairItem.IsDefault = false;
                    RepairItems.Update(repairItem);
                }
                //Make the Required field identical for all items.
                if (row.Required != e.OldRow.Required && repairItem.Required != row.Required)
                {
                    repairItem.Required = row.Required;
                    RepairItems.Update(repairItem);
                }
            }
            //Refresh the UI.
            RepairItems.View.RequestRefresh();
        }

        // Check whether there are any repair items with the same repair item type 
        // and copy the Required value from the previous records
        protected void _(Events.FieldDefaulting<RSSVRepairItem, RSSVRepairItem.required> e)
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
                    line.DefaultWarranty = true;
                    // Insert the data record into
                    // the cache of the Warranty data view.
                    Warranty.Insert(line);
                    // Clear the flag that indicates in the UI whether the cache
                    // contains changes.
                    Warranty.Cache.IsDirty = oldDirty;
                }
            }
        }

        protected virtual void _(Events.RowDeleting<RSSVWarranty> e)
        {
            if (e.Row.DefaultWarranty != true) return;
            if (e.ExternalCall && RepairPrices.Current != null &&
            RepairPrices.Cache.GetStatus(RepairPrices.Current) !=
            PXEntryStatus.Deleted)
            {
                throw new PXException(Messages.DefaultWarrantyCanNotBeDeleted);
            }
        }

        //Make the default warranty unavailable for editing.
        protected virtual void _(Events.RowSelected<RSSVWarranty> e)
        {
            RSSVWarranty line = e.Row;
            if (line == null) return;
            PXUIFieldAttribute.SetEnabled<RSSVWarranty.contractID>(e.Cache, line, line.DefaultWarranty != true);
        }

        #endregion

        //The FBQL constant for the free warranty that is inserted by default
        public const string DefaultWarrantyConstant = "DFLTWARRNT";
        public class defaultWarranty : PX.Data.BQL.BqlString.Constant<defaultWarranty>
        {
            public defaultWarranty()
            : base(DefaultWarrantyConstant)
            {
            }
        }
    }
}