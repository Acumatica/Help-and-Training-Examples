using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Linq;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint : 
        PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {
        #region Data Views
        ...

        public SelectFrom<RSSVWarranty>.
            Where<RSSVWarranty.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVWarranty.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.
            OrderBy<RSSVWarranty.defaultWarranty.Desc>.View
            Warranty;
        #endregion

        ...
    }
}