using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVSetupMaint : PXGraph<RSSVSetupMaint>
  {
        public SelectFrom<RSSVSetup>.View Setup;
        public PXSave<RSSVSetup> Save;
        public PXCancel<RSSVSetup> Cancel;

  }
}