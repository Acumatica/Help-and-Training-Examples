using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVSetupMaint : PXGraph<RSSVSetupMaint>
    {

        public PXSave<RSSVSetup> Save = null!;
        public PXCancel<RSSVSetup> Cancel = null!;

        public SelectFrom<RSSVSetup>.View Setup = null!;
    }
}