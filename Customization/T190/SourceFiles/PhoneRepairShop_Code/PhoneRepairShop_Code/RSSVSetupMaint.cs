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