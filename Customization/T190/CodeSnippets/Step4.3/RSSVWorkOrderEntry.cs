using PX.Data;
using PX.Data.BQL.Fluent;
using System.Linq;
using PX.Data.BQL;
using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
		...
		
		#region Actions
		 
		...
		
		public PXAction<RSSVWorkOrder> CreateInvoiceAction;
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Create Invoice", Enabled = true)]
		protected virtual IEnumerable createInvoiceAction(PXAdapter adapter)
		{
			// Populate a local list variable.
			List<RSSVWorkOrder> list = new List<RSSVWorkOrder>();
			foreach (RSSVWorkOrder order in adapter.Get<RSSVWorkOrder>())
			{
				list.Add(order);
			}

			// Trigger the Save action to save changes in the database.
			Actions.PressSave();

			var workOrder = WorkOrders.Current;
			PXLongOperation.StartOperation(this, delegate () {
				CreateInvoice(workOrder);
			});

			// Return the local list variable.
			return list;
		}

		#endregion
	}
}