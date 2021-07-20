using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.SO;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Views

        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.
            IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems;

        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.
            IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor;

        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup;

        //The view for the calculation of the number of assigned work orders 
        //per employee
        public SelectFrom<RSSVEmployeeWorkOrderQty>.View Quantity;

        #endregion

        #region Constructors

        //The graph constructor
        public RSSVWorkOrderEntry()
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }


        #endregion



        #region Actions

        public PXAction<RSSVWorkOrder> PutOnHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<RSSVWorkOrder> ReleaseFromHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter) => adapter.Get();

        public PXAction<RSSVWorkOrder> Assign;
        [PXProcessButton]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual IEnumerable assign(PXAdapter adapter)
        {
            // Populate a local list variable.
            List<RSSVWorkOrder> list = new List<RSSVWorkOrder>();
            foreach (RSSVWorkOrder order in adapter.Get<RSSVWorkOrder>())
            {
                list.Add(order);
            }
            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
            PXLongOperation.StartOperation(this, delegate () {
                var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                foreach (RSSVWorkOrder workOrder in list)
                {
                    workOrderEntry.Clear();
                    workOrderEntry.AssignOrder(workOrder);
                }
            });
            // Return the local list variable.
            return list;
        }

        public void AssignOrder(RSSVWorkOrder order, bool isMassProcess = false)
        {
            WorkOrders.Current = order;
            //If the assignee is not specified, specify the default employee.
            if (order.Assignee == null)
            {
                //Retrieve the record with the default setting
                RSSVSetup setupRecord = AutoNumSetup.Current;
                order.Assignee = setupRecord.DefaultEmployee;
            }
            //Change the status of the work order.
            order.Status = WorkOrderStatusConstants.Assigned;
            //Update the work order in the cache.
            order = WorkOrders.Update(order);

            //Modify the number of assigned orders for the employee.
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = order.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = 1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save the changes to the database
            Actions.PressSave();
            //Display the message to indicate successful processing.
            if (isMassProcess)
            {
                PXProcessing.SetInfo(string.Format(Messages.WorkOrderAssigned,
                    order.OrderNbr));
            }
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);
            // Trigger the Save action to save changes in the database
            Actions.PressSave();
            return adapter.Get();
        }


        public PXAction<RSSVWorkOrder> AssignToMe;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Assign To Me", Enabled = true)]
        protected virtual void assignToMe()
        {
            // Get the current order from the cache.
            RSSVWorkOrder row = WorkOrders.Current;

            // Assign the contact ID associated with the current user
            row.Assignee = PXAccess.GetContactID();

            // Update the data record in the cache.
            WorkOrders.Update(row);

            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }

        public PXAction<RSSVWorkOrder> UpdateItemPrices;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateItemPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID != null && order.DeviceID != null)
            {
                var repairItems = RepairItems.Select();
                foreach (RSSVWorkOrderItem item in repairItems)
                {
                    RSSVRepairItem origItem = SelectFrom<RSSVRepairItem>.
                            Where<RSSVRepairItem.inventoryID.IsEqual<@P.AsInt>>.View.
                            Select(this, item.InventoryID);
                    if (origItem != null)
                    {
                        item.BasePrice = origItem.BasePrice;
                        RepairItems.Update(item);
                    }
                }

                Actions.PressSave();
            }
        }

        public PXAction<RSSVWorkOrder> UpdateLaborPrices;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateLaborPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID != null && order.DeviceID != null)
            {
                var laborItems = Labor.Select();
                foreach (RSSVWorkOrderLabor labor in laborItems)
                {
                    RSSVLabor origItem = SelectFrom<RSSVLabor>.
                        Where<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>.View.
                        Select(this, labor.InventoryID);
                    if (origItem != null)
                    {
                        labor.DefaultPrice = origItem.DefaultPrice;
                        Labor.Update(labor);
                    }
                }

                Actions.PressSave();
            }
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

        public PXAction<RSSVWorkOrder> ActionsMenuItem;
        [PXButton(SpecialType = PXSpecialButtonType.ActionsFolder)]
        [PXUIField(DisplayName = "Actions")]
        protected virtual IEnumerable actionsMenuItem(PXAdapter adapter)
        {
            return adapter.Get();
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


        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor,
          RSSVWorkOrderLabor.quantity> e)
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


        //Display an error if the priority is too low for the selected service
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;

            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.priority,
                   RSSVWorkOrder.serviceID>(row, originalRow))
            {
                if (row.Priority == WorkOrderPriorityConstants.Low)
                {
                    //Obtain the service record
                    RSSVRepairService service = SelectFrom<RSSVRepairService>.
                        Where<RSSVRepairService.serviceID.IsEqual<@P.AsInt>>.
                            View.Select(this, row.ServiceID);

                    if (service != null && service.PreliminaryCheck == true)
                    {
                        //Display the error for the Priority field
                        WorkOrders.Cache.RaiseExceptionHandling<RSSVWorkOrder.priority>(row,
                            originalRow.Priority,
                            new PXSetPropertyException(Messages.PriorityTooLow));

                        //Assign the proper priority
                        e.NewRow.Priority = WorkOrderPriorityConstants.Medium;
                    }
                }
            }
        }

        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            RSSVWorkOrder row = e.Row;
            if (row == null) return;

            AssignToMe.SetEnabled((row.Status == WorkOrderStatusConstants.ReadyForAssignment ||
                row.Status == WorkOrderStatusConstants.OnHold) &&
                WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
            AssignToMe.SetVisible(row.Assignee != PXAccess.GetContactID());
            UpdateItemPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
            UpdateLaborPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);

            CreateInvoiceAction.SetVisible(
              WorkOrders.Current.Status == WorkOrderStatusConstants.Completed);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null &&
              (WorkOrders.Current.Status == WorkOrderStatusConstants.Completed ||
              WorkOrders.Current.Status == WorkOrderStatusConstants.PendingPayment));
        }

        #endregion

        #region WorkdlowEvents

        //Event handler for a workflow event
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;

        //Event handler for a workflow event
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnInvoiceGotPrepaid;


        #endregion

    }

    public class ARPaymentEvents : PXEntityEvent<ARPayment>.Container<ARPaymentEvents>
    {
        public PXEntityEvent<ARPayment> InvoiceGotPrepaid;
    }

    public class MyEvents : PXEntityEventBase<ARInvoice>.Container<MyEvents>
    {
        public PXEntityEvent<ARInvoice> InvoiceGotPrepaid;
    }

    public class RSSVWorkOrderEntry_Extension : PXGraphExtension<RSSVWorkOrderEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
            Base.ActionsMenuItem.AddMenuAction(Base.CreateInvoiceAction);
        }
    }
}