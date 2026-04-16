using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVRepairPriceMaint :
        PXGraph<RSSVRepairPriceMaint, RSSVRepairPrice>
    {
        #region Data Views
        public SelectFrom<RSSVRepairPrice>.View RepairPrices = null!;

        ////////// The added code
        public SelectFrom<RSSVRepairItem>.
            Where<RSSVRepairItem.serviceID.
                IsEqual<RSSVRepairPrice.serviceID.FromCurrent>.
            And<RSSVRepairItem.deviceID.
                IsEqual<RSSVRepairPrice.deviceID.FromCurrent>>>.View
            RepairItems = null!;
        ////////// The end of added code
        
        public PXFilter<MasterTable> MasterView;
        public PXFilter<DetailsTable> DetailsView;

        [Serializable]
        public class MasterTable : PXBqlTable, IBqlTable
        {

        }

        [Serializable]
        public class DetailsTable : PXBqlTable, IBqlTable
        {

        }

        #endregion
    }
}