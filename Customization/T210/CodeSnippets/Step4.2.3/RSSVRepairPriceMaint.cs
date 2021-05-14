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
        #endregion

        #region Event Handlers
        ...

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
        ...
        #endregion
    }
}