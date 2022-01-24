using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections.Generic;

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

public PXAction<RSSVWorkOrder> CreateInvoiceAction;
[PXButton]
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