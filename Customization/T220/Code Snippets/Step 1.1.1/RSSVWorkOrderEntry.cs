using System;
using PX.Data;

namespace PhoneRepairShop
{
   public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry>
    {
        //The primary view for the Summary area of the form
        public SelectFrom<RSSVWorkOrder>.View WorkOrders;
        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
        Where<RSSVWorkOrderItem.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View RepairItems;
        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
        Where<RSSVWorkOrderLabor.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View Labor;

    }
}