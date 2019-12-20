using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System.Linq;
using System.Collections;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {
        #region Data Views
        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            LeftJoin<InventoryItem>.
                On<InventoryItem.inventoryID.IsEqual<
                    RSSVWorkOrderItem.inventoryID.FromCurrent>>.
            Where<RSSVWorkOrderItem.orderNbr.IsEqual<
                RSSVWorkOrder.orderNbr.FromCurrent>>.
            View RepairItems;

        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            LeftJoin<InventoryItem>.
                On<InventoryItem.inventoryID.IsEqual<
                    RSSVWorkOrderLabor.inventoryID.FromCurrent>>.
            Where<RSSVWorkOrderLabor.orderNbr.IsEqual<
                RSSVWorkOrder.orderNbr.FromCurrent>>.
            View Labor;

        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup;

        //The view for the calculation of the number of assigned work orders per
        //employee
        public SelectFrom<RSSVEmployeeWorkOrderQty>.View Quantity;

        #endregion

        #region Constructor
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
            RSSVWorkOrder order = e.Row;
            if (order == null) return;
            if (WorkOrders.Cache.GetStatus(order) == PXEntryStatus.Inserted &&
            RepairItems.Select().Count == 0 && Labor.Select().Count == 0)
            {
                if (order.DeviceID != null && order.ServiceID != null)
                {
                    //Retrieve the default repair items
                    var repairItems = SelectFrom<RSSVRepairItem>.
                    Where<RSSVRepairItem.deviceID.IsEqual<
                    RSSVWorkOrder.deviceID.FromCurrent>.
                    And<RSSVRepairItem.serviceID.IsEqual<
                    RSSVWorkOrder.serviceID.FromCurrent>>>.View.Select(this);
                    //Insert default repair items
                    if (repairItems != null)
                    {
                        foreach (RSSVRepairItem item in repairItems)
                        {
                            RSSVWorkOrderItem orderItem = RepairItems.Insert(
                            new RSSVWorkOrderItem());
                            orderItem.RepairItemType = item.RepairItemType;
                            orderItem.InventoryID = item.InventoryID;
                            orderItem.BasePrice = item.BasePrice;
                            orderItem.LineNbr = item.LineNbr;
                            RepairItems.Update(orderItem);
                        }
                    }
                    //Retrieve the default labor items
                    var laborItems = SelectFrom<RSSVLabor>.
                    Where<RSSVLabor.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>.
                    And<RSSVLabor.serviceID.IsEqual<
                    RSSVWorkOrder.serviceID.FromCurrent>>>.View.Select(this);
                    //Insert the default labor items
                    if (laborItems != null)
                    {
                        foreach (RSSVLabor item in laborItems)
                        {
                            RSSVWorkOrderLabor orderItem = Labor.Insert(new RSSVWorkOrderLabor());
                            orderItem.InventoryID = item.InventoryID;
                            orderItem.DefaultPrice = item.DefaultPrice;
                            orderItem.Quantity = item.Quantity;
                            orderItem.ExtPrice = item.ExtPrice;
                            Labor.Update(orderItem);
                        }
                    }
                }
            }
        }

        ////Update price and repair item type when inventory ID of repair item is updated.
        //protected void _(Events.FieldUpdated<RSSVWorkOrderItem, RSSVWorkOrderItem.inventoryID> e)
        //{
        //    RSSVWorkOrderItem row = e.Row;

        //    if (row.InventoryID != null)
        //    {
        //        //Use the PXSelector attribute to select the stock items
        //        InventoryItem item =
        //            PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(
        //            e.Cache, row) as InventoryItem;
        //        //Copy the base price from the stock item
        //        row.BasePrice = item.BasePrice;
        //        //Retrieve the extension fields
        //        InventoryItemExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemExt>(item);
        //        if (itemExt != null)
        //        {
        //            // Copy the repair item type from the stock item
        //            row.RepairItemType = itemExt.UsrRepairItemType;

        //        }
        //    }
        //}

        //Change the status based on whether the Hold check box is selected or
        //cleared.
        protected virtual void _(Events.FieldUpdated<RSSVWorkOrder,
            RSSVWorkOrder.hold> e)
        {
            RSSVWorkOrder row = e.Row;
            //If Hold is cleared, specify the status depending
            //on the Prepayment field of the service
            if (row.Hold == false)
            {
                RSSVRepairService service =
                    (RSSVRepairService)SelectFrom<RSSVRepairService>.
                    Where<RSSVRepairService.serviceID.IsEqual<
                        RSSVWorkOrder.serviceID.FromCurrent>>.
                    View.Select(this);
                if (service != null && service.Prepayment == true)
                    e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row, WorkOrderStatusConstants.PendingPayment);                    
                if (service != null && service.Prepayment == false)
                    e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row, WorkOrderStatusConstants.ReadyForAssignment);                   
            }
            //If Hold is selected, change the status to On Hold
            if (row.Hold == true) 
                e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row, WorkOrderStatusConstants.OnHold);
        }       

        //Display an error if a required repair item is missing in a work order
        //for which a user clears the Hold check box.
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;
            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.hold, RSSVWorkOrder.status>(
            row, originalRow))
            {
                if (row.Status == WorkOrderStatusConstants.PendingPayment ||
                row.Status == WorkOrderStatusConstants.ReadyForAssignment)
                {
                    //Select the required repair items for this service and device
                    PXResultset<RSSVRepairItem> repairItems =
                        SelectFrom<RSSVRepairItem>.
                        Where<RSSVRepairItem.deviceID.IsEqual<
                                RSSVWorkOrder.deviceID.FromCurrent>.
                            And<RSSVRepairItem.serviceID.IsEqual<
                                RSSVWorkOrder.serviceID.FromCurrent>>.
                            And<RSSVRepairItem.required.IsEqual<True>>>.
                        AggregateTo<GroupBy<RSSVRepairItem.repairItemType>,
                        Count<RSSVRepairItem.inventoryID>>.View.Select(this);
                    if (repairItems.Any())
                    {
                        foreach (PXResult<RSSVRepairItem> line in repairItems)
                        {
                            //Check whether the required repair items exist
                            //in the work order.
                            var workOrderItems = RepairItems.Select().Where(item =>
                                item.GetItem<RSSVWorkOrderItem>().RepairItemType ==
                                line.GetItem<RSSVRepairItem>().RepairItemType)
                                .ToList();
                            if (workOrderItems.Count == 0)
                            {
                                //Obtain the attribute assigned to
                                //the RSSVWorkOrderItem.RepairItemType field.
                                PXStringListAttribute stringListAttribute =
                                    RepairItems.Cache
                                    .GetAttributesReadonly<
                                        RSSVWorkOrderItem.repairItemType>()
                                        .OfType<PXStringListAttribute>()
                                        .SingleOrDefault();
                                //Obtain the label that corresponds to
                                //the required repair item type.
                                stringListAttribute.ValueLabelDic.TryGetValue(
                                    line.GetItem<RSSVRepairItem>().RepairItemType,
                                    out string label);
                                //Display the error for the status field.
                                WorkOrders.Cache.RaiseExceptionHandling<
                                    RSSVWorkOrder.status>(
                                        row,
                                        row.Status,
                                        new PXSetPropertyException(
                                        Messages.NoRequiredItem, label));
                                //Cancel the change of the status.
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        //Define availability and visibility of actions
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            RSSVWorkOrder row = e.Row;
            if (row == null) return;

            Assign.SetEnabled(row.Status == WorkOrderStatusConstants.ReadyForAssignment);

            Complete.SetEnabled(row.Status == WorkOrderStatusConstants.Assigned);
        }
        #endregion

        #region Actions
        public PXAction<RSSVWorkOrder> Assign;
        //This action handler has been replaced with the next one for the Assign Work Orders processing form.
        //The code has been moved to the AssignOrder method.
        //[PXButton(CommitChanges = true)]
        //[PXUIField(DisplayName = "Assign", Enabled = false)]
        //protected virtual void assign()
        //{           
        //    // Get the current order from the cache
        //    RSSVWorkOrder row = WorkOrders.Current;
        //    // If an Assignee has not been specified
        //    // change the Assignee box value to the default employee value
        //    if (row.Assignee == null)
        //        row.Assignee = AutoNumSetup.Current.DefaultEmployee;
        //    // Change the order status to Assigned
        //    row.Status = WorkOrderStatusConstants.Assigned;
        //    // Update the data record in the cache
        //    WorkOrders.Update(row);
        //    // Trigger the Save action to save changes in the database
        //    Actions.PressSave();
        //}
        [PXProcessButton]
        [PXUIField(DisplayName = "Assign", Enabled = false)]
        protected virtual IEnumerable assign(PXAdapter adapter)
        {
            foreach (RSSVWorkOrder order in adapter.Get())
            {
                Actions.PressSave();
                PXLongOperation.StartOperation(this, delegate ()
                {
                    AssignOrder(order);
                });
                yield return order;
            }
        }


        public void AssignOrder(RSSVWorkOrder order, bool isMassProcess = false)
        {
            WorkOrders.Current = order;
            //If the assignee is not specified, specify the default employee.
            if (order.Assignee == null)
            {
                //Retrieve the record with the default setting
                order.Assignee = AutoNumSetup.Current.DefaultEmployee;
            }
            //Change the status of the work order.
            order.Status = WorkOrderStatusConstants.Assigned;
            //Update the work order in the cache.
            WorkOrders.Update(order);

            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders = new
                RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = order.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = 1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save changes to the database
            Actions.PressSave();
            //Display the message for successful processing.
            if (isMassProcess)
            {
                PXProcessing.SetInfo(String.Format(
                Messages.WorkOrderAssigned, order.OrderNbr));
            }
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual void complete()
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;

            // Change the order status to Assigned
            row.Status = WorkOrderStatusConstants.Completed;
            // Set the Date Completed value
            row.DateCompleted = this.Accessinfo.BusinessDate;

            // Update the data record in the cache
            WorkOrders.Update(row);

            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders = new
                RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save changes in the database
            Actions.PressSave();
        }
        
        #endregion
    }
}