using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        ...

        #region Actions
        public PXAction<RSSVWorkOrder> ReleaseFromHold;
        [PXButton(), PXUIField(DisplayName = "Remove Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter)
           => adapter.Get();
        #endregion

        ...

    }
}