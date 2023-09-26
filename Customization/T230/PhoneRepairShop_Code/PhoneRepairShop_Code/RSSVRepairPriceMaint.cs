using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Linq;
using PX.Objects.CT;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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

        public SelectFrom<RSSVWarranty>.
            Where<RSSVWarranty.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVWarranty.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
            OrderBy<RSSVWarranty.defaultWarranty.Desc>.View
            Warranty;

        //The view for the default warranty
        public SelectFrom<ContractTemplate>.
            Where<ContractTemplate.contractCD.IsEqual<defaultWarranty>>.
            View DefaultWarranty;
        #endregion

        private static void ValidatePrices(RSSVRepairPrice repairPriceItem)
        {
            // Create an instance of the RSSVRepairPriceMaint graph and set the Current property of its RepairPrices view.
            var priceMaint = PXGraph.CreateInstance<RSSVRepairPriceMaint>();
            priceMaint.RepairPrices.Current = priceMaint.RepairPrices.Search<RSSVRepairPrice.serviceID, RSSVRepairPrice.deviceID> (repairPriceItem.ServiceID, repairPriceItem.DeviceID);

            // Set a delay to mimic connecting to an external service to validate the 
            // repair item prices.
            // In a real world scenario, you would connect to an actual external service and 
            // make an API request to validate the prices for the repair items.
            Thread.Sleep(3000);

            // Update the Price Validated field for each repair item on the Repair Items tab:
            // Here we are assuming that the validation was successful from the external service 
            // and are setting IsPriceValidated to true for each repair item.     
            foreach (RSSVRepairItem item in priceMaint.RepairItems.Select())
            {
                // Set IsPriceValidated to true for each repair item.
                item.IsPriceValidated = true;
                // Update the cache with the above change for each repair item.
                priceMaint.RepairItems.Update(item);
            }
            // Trigger the Save action to save the changes stored in the cache to the database.
            priceMaint.Actions.PressSave();
        }

        #region Actions
        public PXAction<RSSVRepairPrice> ValidateItemPrices;
        [PXButton(DisplayOnMainToolbar = false, CommitChanges = true)]
        [PXUIField(DisplayName = "Validate Prices", Enabled = true)]
        protected virtual IEnumerable validateItemPrices(PXAdapter adapter)
        {
            // Populate a local list variable.
            List<RSSVRepairPrice> list = new List<RSSVRepairPrice>();
            foreach (RSSVRepairPrice repairItemPrice in adapter.Get<RSSVRepairPrice>())
            {
                list.Add(repairItemPrice);
            }

            // Trigger the Save action to save changes in the database.
            Actions.PressSave();

            var repairPriceItem = RepairPrices.Current;
            //Execute ValidatePrices method asynchronously using PXLongOperation.StartOperation
            PXLongOperation.StartOperation(this, () => ValidatePrices(repairPriceItem));

            // Return the local list variable.
            return list;
        }
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
                InventoryItemCurySettings curySettings = InventoryItemCurySettings.PK.Find(
                    this, item.InventoryID, Accessinfo.BaseCuryID ?? "USD");
                //Copy the base price from the stock item to the row.
                e.NewValue = curySettings.BasePrice;
            }
        }

        //When Repair Item Type is updated,
        //clear the values of the Inventory ID and Default columns and
        //trigger FieldDefaulting for the Required column.
        protected void _(Events.FieldUpdated<RSSVRepairItem, RSSVRepairItem.repairItemType> e)
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

        //Update the IsDefault and Required fields of other records 
        //with the same repair item type when these fields are updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault,
                                     RSSVRepairItem.required>(e.Row, e.OldRow)) 
                return;

            RSSVRepairItem row = e.Row;
            //Use LINQ to select the repair items 
            // with the same repair item type as in the updated row.
            var repairItems = RepairItems.Select()
                .Where(item => item.GetItem<RSSVRepairItem>().RepairItemType == 
                row.RepairItemType);
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

        //Prevent removal of the default warranty.
        protected virtual void _(Events.RowDeleting<RSSVWarranty> e)
        {
            if (e.Row.DefaultWarranty != true) return;

            if (e.ExternalCall && RepairPrices.Current != null &&
                RepairPrices.Cache.GetStatus(RepairPrices.Current) != PXEntryStatus.Deleted)
            {
                throw new PXException(Messages.DefaultWarrantyCanNotBeDeleted);
            }
        }

        //Make the default warranty unavailable for editing.
        protected virtual void _(Events.RowSelected<RSSVWarranty> e)
        {
            RSSVWarranty line = e.Row;
            if (line == null) return;
            PXUIFieldAttribute.SetEnabled<RSSVWarranty.contractID>(e.Cache,
                line, line.DefaultWarranty != true);
        }
        #endregion

        #region Constant
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