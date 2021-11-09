using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVSetupMaint : PXGraph<RSSVSetupMaint>
    {

        public PXSave<RSSVSetup> Save;
        public PXCancel<RSSVSetup> Cancel;
		
        public SelectFrom<RSSVSetup>.View Setup;


        public PXFilter<MasterTable> MasterView;
        public PXFilter<DetailsTable> DetailsView;

        [Serializable]
        public class MasterTable : IBqlTable
        {

        }

        [Serializable]
        public class DetailsTable : IBqlTable
        {

        }


    }
}