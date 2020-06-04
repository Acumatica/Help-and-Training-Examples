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
            Complete.SetEnabled(row.Status == WorkOrderStatusConstants.Assigned);
        }

        #endregion


        #region Actions

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

            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save changes in the database.
            Actions.PressSave();
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
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders = new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = order.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = 1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save the changes to the database
            Actions.PressSave();

            //Display the message to indicate successful processing.
            if (isMassProcess)
            {
                PXProcessing.SetInfo(string.Format(Messages.WorkOrderAssigned, order.OrderNbr));
            }
        }

        #endregion
    }
}