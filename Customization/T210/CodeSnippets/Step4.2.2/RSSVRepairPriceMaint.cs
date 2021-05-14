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
        ...

        //The view for the default warranty
        public SelectFrom<ContractTemplate>.
            Where<ContractTemplate.contractCD.IsEqual<defaultWarranty>>.
            View DefaultWarranty;
        #endregion

        #region Event Handlers
        ...

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
        #endregion

        #region Constant
        //The fluent BQL constant for the free warranty that is inserted by default
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