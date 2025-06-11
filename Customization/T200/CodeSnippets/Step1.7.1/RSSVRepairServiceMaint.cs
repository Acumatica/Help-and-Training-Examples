using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {
////////// The added code
    protected void _(Events.FieldUpdated<RSSVRepairService, 
      RSSVRepairService.walkInService> e)
    { 
      var row = e.Row;
      row.PreliminaryCheck = !(row.WalkInService == true);
    }
////////// The end of added code

    public PXSave<RSSVRepairService> Save = null!;
    public PXCancel<RSSVRepairService> Cancel = null!;

    public SelectFrom<RSSVRepairService>.View RepairService = null!;
  }
}