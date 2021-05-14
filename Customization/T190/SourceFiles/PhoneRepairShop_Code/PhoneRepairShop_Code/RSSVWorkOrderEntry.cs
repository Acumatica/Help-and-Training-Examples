using System.Collections;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.IN;
using PX.Objects.AR;
using PX.Objects.SO;
using System.Collections.Generic;
using PX.Objects.Common.GraphExtensions.Abstract.DAC;
using PX.Objects.CR;

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

        #region Event Handlers

        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;

        #endregion


        #region Actions

        // T220 section
        public PXAction<RSSVWorkOrder> putOnHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable PutOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<RSSVWorkOrder> releaseFromHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable ReleaseFromHold(PXAdapter adapter) => adapter.Get();


        // T230 section
        public PXAction<RSSVWorkOrder> assign;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual void Assign()
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

        public PXAction<RSSVWorkOrder> complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable Complete(PXAdapter adapter) => adapter.Get();

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

        //public PXAction<RSSVWorkOrder> EmailOrder;
        //[PXButton(CommitChanges = true)]
        //[PXUIField(DisplayName = "Email Order", Enabled = true)]
        //protected virtual void emailOrder()
        //{
        //    var order = WorkOrders.Current;
        //    var parameters = new Dictionary<string, string>();
        //    parameters["SOOrder.OrderNbr"] = order.OrderNbr;
        //    parameters["SOOrder.DateCreated"] = order.DateCreated.ToString();
        //    var activity = RWOActivities();
        //    CRActivityList<RSSVWorkOrder>.SendNotification(ARNotificationSource.Customer, "Repair Work Order", this.Accessinfo.BranchID, parameters);
        //}

        //public sealed class RWOActivities : CRActivityList<RSSVWorkOrder> 
        //{
        //    public RWOActivities(PXGraph graph);
        //}

        #endregion

        #region Constructors

        //The graph constructor
        public RSSVWorkOrderEntry()
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }

    #endregion

        #region Events 

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

        //Display an error if a required repair item is missing in a work order
        //for which a user clears the Hold check box.
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;

            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.priority, RSSVWorkOrder.serviceID>(row, originalRow))
            {
                if (row.Priority == WorkOrderPriorityConstants.Low)
                {
                    //Obtain the service record
                    RSSVRepairService service = SelectFrom<RSSVRepairService>.
                        Where<RSSVRepairService.serviceID.IsEqual<@P.AsInt>>.View.Select(this, row.ServiceID);

                    if (service != null && service.PreliminaryCheck == true)
                    {
                        //Display the error for the priority field.
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
            CreateInvoiceAction.SetVisible(WorkOrders.Current.Status == WorkOrderStatusConstants.Completed);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
        }


        #endregion


    }
}