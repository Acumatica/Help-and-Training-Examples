using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;
using PX.Objects.CS;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order Preferences")]
    [PXPrimaryGraph(typeof(RSSVSetupMaint))]
    public class RSSVSetup : IBqlTable
    {
        ...
    }
}
