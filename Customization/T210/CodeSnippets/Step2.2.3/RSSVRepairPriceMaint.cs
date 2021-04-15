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

        //Update the IsDefault field of other records with the same repair item type 
        //when the IsDefault field is updated.
        protected void _(Events.RowUpdated<RSSVRepairItem> e)
        {
            // Make sure the handler runs only when the IsDefault field is edited.
            if (e.Cache.ObjectsEqual<RSSVRepairItem.isDefault>(e.Row, e.OldRow)) return;

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