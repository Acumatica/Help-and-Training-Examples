using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {
    ...

    public PXSave<RSSVRepairService> Save;
    public PXCancel<RSSVRepairService> Cancel;

    public SelectFrom<RSSVRepairService>.View RepairService;

    ...
  }
}