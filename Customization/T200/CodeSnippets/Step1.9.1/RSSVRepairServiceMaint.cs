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
    
      if (row.WalkInService == true)
      {
        row.PreliminaryCheck = false;
      }
      else
      {
        row.PreliminaryCheck = true;
      }
      
    }
/////////// The added code
    protected void _(Events.FieldUpdated<RSSVRepairService,
            RSSVRepairService.preliminaryCheck> e)
    {
        var row = e.Row;
        if (row.PreliminaryCheck == true)
        {
            row.WalkInService = false;
        }
        else
        {
            row.WalkInService = true;
        }
    }
/////////// The end of added code
    public PXSave<RSSVRepairService> Save;
    public PXCancel<RSSVRepairService> Cancel;
	 
	public SelectFrom<RSSVRepairService>.View RepairService;
  }
}