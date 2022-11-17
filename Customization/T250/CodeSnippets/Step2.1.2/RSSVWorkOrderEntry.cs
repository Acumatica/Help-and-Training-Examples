using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections;
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

        ////////// The added code
        public SelectFrom<RSSVWorkOrderPayment>.
            Where<RSSVWorkOrderPayment.invoiceNbr.IsEqual<
                RSSVWorkOrder.invoiceNbr.FromCurrent>>.
            View Payments;
        ////////// The end of added code

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
        [PXButton, PXUIField(DisplayName = "Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<RSSVWorkOrder> ReleaseFromHold;
        [PXButton, PXUIField(DisplayName = "Remove Hold",
          MapEnableRights = PXCacheRights.Select,
          MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter) => adapter.Get();


        public PXAction<RSSVWorkOrder> AssignToMe;
        [PXButton]
        [PXUIField(DisplayName = "Assign to Me", Enabled = true)]
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
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateItemPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID == null || order.DeviceID == null) return;
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

        public PXAction<RSSVWorkOrder> UpdateLaborPrices;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateLaborPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID == null || order.DeviceID == null) return;
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


        // Manage visibility and availability of the actions.
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
                WorkOrders.Current.Status == WorkOrderStatusConstants.Completed);
        }
        #endregion

        #region Workflow Implementation
        //Event handler for a workflow event
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;

        public PXAction<RSSVWorkOrder> Assign;
        [PXButton]
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
            // row.Status = WorkOrderStatusConstants.Assigned;

            // Update the data record in the cache.
            WorkOrders.Update(row);

            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter) => adapter.Get();
        #endregion
    }
}