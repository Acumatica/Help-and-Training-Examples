using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {

    protected void _(Events.FieldUpdated<RSSVRepairService, RSSVRepairService.walkInService> e)
    {
        var row = e.Row;
        row.PreliminaryCheck = !(row.WalkInService == true);
        }

    protected void _(Events.FieldUpdated<RSSVRepairService, RSSVRepairService.preliminaryCheck> e)
    {
        var row = e.Row;
        row.WalkInService = !(row.PreliminaryCheck == true);
        }

    public SelectFrom<RSSVRepairService>.View RepairService = null!;

    public PXSave<RSSVRepairService> Save = null!;
    public PXCancel<RSSVRepairService> Cancel = null!;


  }
}