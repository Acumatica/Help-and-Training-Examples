using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Views
        ...
        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup;
        #endregion
                                    
        #region Graph constructor
        public RSSVWorkOrderEntry()
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }
        #endregion
        ...
    }
}