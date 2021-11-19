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
			if (row.WalkInService == true)
			{
			row.PreliminaryCheck = false;
			}
			else
			{
			row.PreliminaryCheck = true;
			}
		}

		protected void _(Events.FieldUpdated<RSSVRepairService, RSSVRepairService.preliminaryCheck> e)
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

		public SelectFrom<RSSVRepairService>.View RepairService;

		public PXSave<RSSVRepairService> Save;
		public PXCancel<RSSVRepairService> Cancel;


	}
}