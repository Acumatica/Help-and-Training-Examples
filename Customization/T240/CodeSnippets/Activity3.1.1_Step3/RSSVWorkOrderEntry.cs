using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Objects.AR;
using System.Collections.Generic;
using PX.Objects.SO;
using PX.Data.WorkflowAPI;

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

        #region Event Handlers 

        //Copy repair items and labor items from the Services and Prices form.
        protected virtual void _(Events.RowUpdated<RSSVWorkOrder> e)
        {
            if (WorkOrders.Cache.GetStatus(e.Row) != PXEntryStatus.Inserted ||
                e.Cache.ObjectsEqual<RSSVWorkOrder.serviceID, RSSVWorkOrder.deviceID>(e.Row, e.OldRow))
                return;

            if (e.Row.ServiceID == null || e.Row.DeviceID == null ||
                IsCopyPasteContext || RepairItems.Select().Count != 0 ||
                Labor.Select().Count != 0)
                return;

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
            if (row.InventoryID == null) return;
            //Use the PXSelector attribute to select the stock item.
            InventoryItem item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
            //Retrieve the base price for the stock item.
            InventoryItemCurySettings curySettings = InventoryItemCurySettings.PK.Find(
                this, item.InventoryID, Accessinfo.BaseCuryID ?? "USD");
            //Copy the base price from the stock item to the row.
            e.NewValue = curySettings.BasePrice;
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
                throw new PXSetPropertyException(e.Row, Messages.QuantityCannotBeNegative);
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
                        new PXSetPropertyException(e.Row, Messages.QuantityTooSmall, PXErrorLevel.Warning));
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
                            new PXSetPropertyException(row, Messages.PriorityTooLow));

                        //Assign the proper priority
                        e.NewRow.Priority = WorkOrderPriorityConstants.Medium;
                    }
                }
            }
        }
        #endregion

        #region Actions
        public PXAction<RSSVWorkOrder> ReleaseFromHold;
        [PXButton(), PXUIField(DisplayName = "Remove Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter)
           => adapter.Get();

        public PXAction<RSSVWorkOrder> PutOnHold;
        [PXButton, PXUIField(DisplayName = "Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();

        public static void AssignOrders(List<RSSVWorkOrder> list,
            bool isMassProcess = false)
        {
            ////////// The added code
            // The result set to run the report on.
            PXReportResultset assignedOrders =
                new PXReportResultset(typeof(RSSVWorkOrder));
            ////////// The end of added code
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            // Modify the code so that it works with the list of repair work
            // orders obtained from the input parameter of the method.
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    continue;
                RSSVWorkOrder workOrder = list[i];
                // To handle errors that might occur during the processing,
                // enclose the processing code in the try statement.
                try
                {
                    workOrder.Assignee = workOrder.AssignTo;
                    workOrderEntry.Clear();
                    workOrderEntry.WorkOrders.Current = workOrder;
                    // If the assignee is not specified,
                    // specify the default employee.
                    if (workOrder.Assignee == null)
                    {
                        // Retrieve the record with the default setting
                        RSSVSetup setupRecord =
                        workOrderEntry.AutoNumSetup.Current;
                        workOrder.Assignee = setupRecord.DefaultEmployee;
                    }
                    // Update the work order in the cache.
                    workOrderEntry.WorkOrders.Update(workOrder);
                    //Modify the number of assigned orders for the employee.
                    RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                        new RSSVEmployeeWorkOrderQty();
                    employeeNbrOfOrders.UserID = workOrder.Assignee;
                    employeeNbrOfOrders.NbrOfAssignedOrders = 1;
                    workOrderEntry.Quantity.Insert(employeeNbrOfOrders);
                    // Trigger the Save action to save the changes
                    // to the database.
                    workOrderEntry.Actions.PressSave();
                    // Display the message to indicate successful processing.
                    if (isMassProcess)
                    {
                        PXProcessing<RSSVWorkOrder>.SetInfo(i,
                        string.Format(Messages.WorkOrderAssigned,
                        workOrder.OrderNbr));
                    }
                    ////////// The added code
                    // Add to the result set the order 
                    // that has been successfully assigned.
                    if (workOrder.Status == WorkOrderStatusConstants.Assigned)
                    {
                        assignedOrders.Add(workOrder);
                    }
                    ////////// The end of added code
                }
                catch (Exception e)
                {
                    // Return the processing result for each repair work order
                    // to the UI.
                    PXProcessing<RSSVWorkOrder>.SetError(i, e);
                }
            }
            ////////// The added code
            if (assignedOrders.GetRowCount() > 0 && isMassProcess)
            {
                throw new PXReportRequiredException(assignedOrders, "RS601000",
                                                    Messages.ReportRS601000Title);
            }
            ////////// The end of added code
        }

        public PXAction<RSSVWorkOrder> Assign;
        // Use the PXProcessButton attribute instead of the PXButton attribute 
        // to indicate that the action will be used on the processing form.
        [PXProcessButton]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        // Use the signature of the action handler that 
        // returns IEnumerable. 
        protected virtual IEnumerable assign(PXAdapter adapter)
        {
            bool isMassProcess = adapter.MassProcess;
            // Populate a local list variable.
            List<RSSVWorkOrder> list = new List<RSSVWorkOrder>();
            foreach (RSSVWorkOrder order in adapter.Get<RSSVWorkOrder>())
            {
                list.Add(order);
            }
            // Trigger the Save action to save changes in the database.
            // Call Save.Press() instead of Actions.PressSave() because 
            // the action is used in a workflow and 
            // starts a long-running operation.
            Save.Press();

            // Start execution of the processing method in a separate thread.
            PXLongOperation.StartOperation(this, delegate ()
            {
                AssignOrders(list, isMassProcess);
            });

            // Return the local list variable.
            return list;
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.UserID = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);
            // Trigger the Save action to save changes in the database
            Actions.PressSave();
            return adapter.Get();
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
        #endregion

        #region Workflow Event Handlers 
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;

        public PXWorkflowEventHandler<RSSVWorkOrder, ARRegister> OnInvoiceGotPrepaid;
        #endregion
    }
}