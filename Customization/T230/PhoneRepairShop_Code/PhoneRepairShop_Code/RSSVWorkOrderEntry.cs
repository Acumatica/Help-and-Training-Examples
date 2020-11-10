using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using System.Linq;
using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Data Views

        //The primary view for the Summary area of the form
        public SelectFrom<RSSVWorkOrder>.View WorkOrders;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems;
        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor;

        
        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup;

        #endregion


        #region Constructors

        //The graph constructor
        public RSSVWorkOrderEntry()
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }

        #endregion


        #region Event Handlers

        //Copy repair items and labor items from the Services and Prices form.
        protected virtual void _(Events.RowUpdated<RSSVWorkOrder> e)
        {
            if (WorkOrders.Cache.GetStatus(e.Row) == PXEntryStatus.Inserted &&
                !e.Cache.ObjectsEqual<RSSVWorkOrder.serviceID, RSSVWorkOrder.deviceID>(e.Row, e.OldRow))
            {
                if (e.Row.ServiceID != null && e.Row.DeviceID != null &&
                    !IsCopyPasteContext && RepairItems.Select().Count == 0 &&
                    Labor.Select().Count == 0)
                {
                    //Retrieve the default repair items
                    var repairItems = SelectFrom<RSSVRepairItem>.
                        Where<RSSVRepairItem.serviceID.IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                            And<RSSVRepairItem.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>
                        .View.Select(this);
                    //Insert default repair items
                    foreach (RSSVRepairItem item in repairItems)
                    {
                        RSSVWorkOrderItem orderItem = RepairItems.Insert();
                        orderItem.RepairItemType = item.RepairItemType;
                        orderItem.InventoryID = item.InventoryID;
                        orderItem.BasePrice = item.BasePrice;
                        RepairItems.Update(orderItem);
                    }

                    //Retrieve the default labor items
                    var laborItems = SelectFrom<RSSVLabor>.
                        Where<RSSVLabor.serviceID.IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                            And<RSSVLabor.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>
                        .View.Select(this);
                    //Insert the default labor items
                    foreach (RSSVLabor item in laborItems)
                    {
                        RSSVWorkOrderLabor orderItem = new RSSVWorkOrderLabor();
                        orderItem.InventoryID = item.InventoryID;
                        orderItem = Labor.Insert(orderItem);
                        orderItem.DefaultPrice = item.DefaultPrice;
                        orderItem.Quantity = item.Quantity;
                        orderItem.ExtPrice = item.ExtPrice;
                        Labor.Update(orderItem);
                    }
                }
            }
        }

        //Update price and repair item type when inventory ID of repair item is updated.
        protected void _(Events.FieldUpdated<RSSVWorkOrderItem, RSSVWorkOrderItem.inventoryID> e)
        {
            RSSVWorkOrderItem row = e.Row;
            if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                InventoryItem item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();
                row.RepairItemType = itemExt.UsrRepairItemType;
            }
            e.Cache.SetDefaultExt<RSSVWorkOrderItem.basePrice>(e.Row);
        }

        protected void _(Events.FieldDefaulting<RSSVWorkOrderItem, RSSVWorkOrderItem.basePrice> e)
        {
            RSSVWorkOrderItem row = e.Row;
            if (row.InventoryID != null)
            {
                //Use the PXSelector attribute to select the stock item.
                InventoryItem item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the base price from the stock item to the row.
                e.NewValue = item.BasePrice;
            }
        }

        //Change the status based on whether the Hold check box is selected or cleared.
        protected virtual void _(Events.FieldUpdated<RSSVWorkOrder, RSSVWorkOrder.hold> e)
        {
            //If Hold is selected, change the status to On Hold
            if (e.Row.Hold == true)
            {
                e.Row.Status = WorkOrderStatusConstants.OnHold;
            }
            else if (e.Row.ServiceID != null)
            {
                RSSVRepairService service = PXSelectorAttribute.Select<RSSVWorkOrder.serviceID>(
                    e.Cache, e.Row) as RSSVRepairService;

                //If Hold is cleared, specify the status 
                // depending on the Prepayment field of the service
                if (service != null)
                {
                    string newStatus;
                    if (service.Prepayment == true)
                    {
                        newStatus = WorkOrderStatusConstants.PendingPayment;
                    }
                    else
                    {
                        newStatus = WorkOrderStatusConstants.ReadyForAssignment;
                    }
                    e.Row.Status = newStatus;
                }
            }
        }

        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor, RSSVWorkOrderLabor.quantity> e)
        {
            if (e.Row == null || e.NewValue == null) return;

            if ((decimal)e.NewValue < 0)
            {
                //Throwing an exception to cancel the assignment of the new value to the field
                throw new PXSetPropertyException(Messages.QuantityCannotBeNegative);
            }

            var workOrder = WorkOrders.Current;
            if (workOrder != null)
            {
                //Retrieving the default labor item related to the work order labor
                RSSVLabor labor = SelectFrom<RSSVLabor>.
                    Where<RSSVLabor.serviceID.IsEqual<@P.AsInt>.
                        And<RSSVLabor.deviceID.IsEqual<@P.AsInt>>.
                        And<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>>
                    .View.Select(this, workOrder.ServiceID, workOrder.DeviceID, e.Row.InventoryID);
                if (labor != null && (decimal)e.NewValue < labor.Quantity)
                {
                    //Correcting the LineQty value
                    e.NewValue = labor.Quantity;
                    //Raising the ExceptionHandling event for the Quantity field
                    //to attach the exception object to the field
                    e.Cache.RaiseExceptionHandling<RSSVWorkOrderLabor.quantity>(e.Row, e.NewValue,
                        new PXSetPropertyException(Messages.QuantityToSmall, PXErrorLevel.Warning));
                }
            }
        }

        //Display an error if a required repair item is missing in a work order
        //for which a user clears the Hold check box.
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;

            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.hold, RSSVWorkOrder.status>(row, originalRow))
            {
                if (row.Status == WorkOrderStatusConstants.PendingPayment ||
                    row.Status == WorkOrderStatusConstants.ReadyForAssignment)
                {
                    //Select the required repair items for this service and device
                    PXResultset<RSSVRepairItem> repairItems = SelectFrom<RSSVRepairItem>.
                        Where<RSSVRepairItem.serviceID.IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                            And<RSSVRepairItem.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>.
                            And<RSSVRepairItem.required.IsEqual<True>>>.
                        AggregateTo<GroupBy<RSSVRepairItem.repairItemType>>.View.Select(this);

                    foreach (RSSVRepairItem line in repairItems)
                    {
                        //Check whether at least one repair item of the required type
                        // exists in the work order.
                        var workOrderItemsExist = RepairItems.Select().AsEnumerable()
                            .Any(item => item.GetItem<RSSVWorkOrderItem>().RepairItemType
                                == line.RepairItemType);

                        if (!workOrderItemsExist)
                        {
                            //Obtain the attribute assigned to 
                            // the RSSVWorkOrderItem.RepairItemType field.
                            var stringListAttribute = RepairItems.Cache
                                .GetAttributesReadonly<RSSVWorkOrderItem.repairItemType>()
                                .OfType<PXStringListAttribute>()
                                .SingleOrDefault();
                            //Obtain the label that corresponds to the required repair item type.
                            stringListAttribute.ValueLabelDic.TryGetValue(line.RepairItemType,
                                out string label);
                            //Display the error for the status field.
                            WorkOrders.Cache.RaiseExceptionHandling<RSSVWorkOrder.status>(row,
                                originalRow.Status,
                                new PXSetPropertyException(Messages.NoRequiredItem, label));

                            //Cancel the change of the status.
                            e.Cancel = true;

                            break;
                        }
                    }
                }
            }
        }

        //Enable the Assign action when the Status is ReadyForAssignment
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            RSSVWorkOrder row = e.Row;
            if (row == null) return;
            Assign.SetEnabled(row.Status == WorkOrderStatusConstants.ReadyForAssignment &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
            Complete.SetEnabled(row.Status == WorkOrderStatusConstants.Assigned &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);

            CreateInvoiceAction.SetVisible(WorkOrders.Current.Status ==
                WorkOrderStatusConstants.PendingPayment ||
                WorkOrders.Current.Status == WorkOrderStatusConstants.Completed
                && WorkOrders.Current.InvoiceNbr == null);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
        }

        #endregion


        #region Actions

        public PXAction<RSSVWorkOrder> Assign;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual void assign()
        {
            // Get the current order from the cache.
            RSSVWorkOrder row = WorkOrders.Current;
            // If an Assignee has not been specified,
            // change the Assignee box value to the default employee value.
            if (row.Assignee == null)
                row.Assignee = AutoNumSetup.Current.DefaultEmployee;
            // Change the order status to Assigned.
            row.Status = WorkOrderStatusConstants.Assigned;
            // Update the data record in the cache.
            WorkOrders.Update(row);
            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual void complete()
        {
            // Get the current order from the cache.
            RSSVWorkOrder row = WorkOrders.Current;
            // Change the order status to Completed.
            row.Status = WorkOrderStatusConstants.Completed;
            // Assign the current date to the DateCompleted field.
            row.DateCompleted = this.Accessinfo.BusinessDate;
            // Update the data record in the cache.
            WorkOrders.Update(row);
            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }

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