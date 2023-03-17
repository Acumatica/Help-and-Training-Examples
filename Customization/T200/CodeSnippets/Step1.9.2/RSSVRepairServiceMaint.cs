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
    ////////// The end of added code
        row.PreliminaryCheck = !(row.WalkInService == true);
    }

    protected void _(Events.FieldUpdated<RSSVRepairService,
             RSSVRepairService.preliminaryCheck> e)
        {
            var row = e.Row;
            row.WalkInService = !(row.PreliminaryCheck == true);
        }

		public PXSave<RSSVRepairService> Save;
		public PXCancel<RSSVRepairService> Cancel;
		
		public SelectFrom<RSSVRepairService>.View RepairService;
  }
}