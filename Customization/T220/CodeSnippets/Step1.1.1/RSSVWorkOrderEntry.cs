using PX.Data;
using PX.Data.BQL.Fluent;
using System;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry,
        RSSVWorkOrder>
    {
        #region Views

        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders = null!;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems = null!;

        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor = null!;

        #endregion
        
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
    }
}