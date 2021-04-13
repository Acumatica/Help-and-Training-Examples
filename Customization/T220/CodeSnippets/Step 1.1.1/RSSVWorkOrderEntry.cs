using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Views

        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems;

        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor;

        #endregion

        #region Actions
        
        public PXAction<RSSVWorkOrder> putOnHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable PutOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<RSSVWorkOrder> releaseFromHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable ReleaseFromHold(PXAdapter adapter) => adapter.Get();

        #endregion

		// code generated from the Customization Project Editor


    }
}