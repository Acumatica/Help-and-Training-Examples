using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {

    protected void RSSVRepairService_WalkInService_FieldUpdated(PXCache cache, 
			PXFieldUpdatedEventArgs e)
    {
      
      var row = (RSSVRepairService)e.Row;
      row.PreliminaryCheck = !(row.WalkInService == true);
      
    }
/////////// The added code
    protected void _(Events.FieldUpdated<RSSVRepairService,
            RSSVRepairService.preliminaryCheck> e)
    {
        var row = e.Row;
        row.WalkInService = !(row.PreliminaryCheck == true);
    }
/////////// The end of added code
    public PXSave<RSSVRepairService> Save = null!;
    public PXCancel<RSSVRepairService> Cancel = null!;
	 
	public SelectFrom<RSSVRepairService>.View RepairService = null!;
  }
}