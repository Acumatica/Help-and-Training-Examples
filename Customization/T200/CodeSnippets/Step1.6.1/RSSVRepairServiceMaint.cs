using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
  public class RSSVRepairServiceMaint : PXGraph<RSSVRepairServiceMaint>
  {
    ...

    protected void RSSVRepairService_WalkInService_FieldUpdated(
          PXCache cache, PXFieldUpdatedEventArgs e)
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

    ...
  }
}