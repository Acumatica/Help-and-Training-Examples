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
		
		private static void CreateInvoice(RSSVWorkOrder workOrder)
		{
			using (var ts = new PXTransactionScope())
			{
				// Create an instance of the SOInvoiceEntry graph.
				var invoiceEntry = PXGraph.CreateInstance<SOInvoiceEntry>();
				// Initialize the summary of the invoice.
				var doc = new ARInvoice()
				{
					DocType = ARDocType.Invoice
				};
				doc = invoiceEntry.Document.Insert(doc);
				doc.CustomerID = workOrder.CustomerID;
				invoiceEntry.Document.Update(doc);

				// Create an instance of the RSSVWorkOrderEntry graph.
				var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
				workOrderEntry.WorkOrders.Current = workOrder;

				// Add the lines associated with the repair items
				// (from the Repair Items tab).
				foreach (RSSVWorkOrderItem line in workOrderEntry.RepairItems.Select())
				{
					var repairTran = invoiceEntry.Transactions.Insert();
					repairTran.InventoryID = line.InventoryID;
					repairTran.Qty = 1;
					repairTran.CuryUnitPrice = line.BasePrice;
					invoiceEntry.Transactions.Update(repairTran);
				}
				// Add the lines associated with labor (from the Labor tab).
				foreach (RSSVWorkOrderLabor line in workOrderEntry.Labor.Select())
				{
					var laborTran = invoiceEntry.Transactions.Insert();
					laborTran.InventoryID = line.InventoryID;
					laborTran.Qty = line.Quantity;
					laborTran.CuryUnitPrice = line.DefaultPrice;
					laborTran.CuryExtPrice = line.ExtPrice;
					invoiceEntry.Transactions.Update(laborTran);
				}

				// Save the invoice to the database.
				invoiceEntry.Actions.PressSave();

				// Assign the generated invoice number and save the changes.
				workOrder.InvoiceNbr = invoiceEntry.Document.Current.RefNbr;
				workOrderEntry.WorkOrders.Update(workOrder);
				workOrderEntry.Actions.PressSave();

				ts.Complete();
			}
		}

		#endregion
	}
}