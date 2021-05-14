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
		
		#region Event Handlers
		 
		...
		
		protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            ...

            CreateInvoiceAction.SetVisible(WorkOrders.Current.Status == WorkOrderStatusConstants.PendingPayment ||
               WorkOrders.Current.Status == WorkOrderStatusConstants.Completed
               && WorkOrders.Current.InvoiceNbr == null);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null);

        }

		#endregion
	}
}