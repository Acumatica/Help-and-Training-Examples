using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {
////////// The added code
    public PXSave<RSSVRepairService> Save;
    public PXCancel<RSSVRepairService> Cancel;

    public SelectFrom<RSSVRepairService>.View RepairService;
////////// The end of added code
   
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