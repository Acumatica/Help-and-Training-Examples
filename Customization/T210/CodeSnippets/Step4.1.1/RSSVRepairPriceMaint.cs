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

        public SelectFrom<RSSVLabor>.
            Where<RSSVLabor.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>.
            And<RSSVLabor.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>>>.View 
            Labor;
        #endregion

        ...
    }
}