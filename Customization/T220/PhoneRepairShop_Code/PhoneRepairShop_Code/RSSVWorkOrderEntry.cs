using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using System.Linq;

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
        #endregion

        #region Graph Constructor
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

        //Update price and repair item type when inventory ID of repair item is updated.
        protected void _(Events.FieldUpdated<RSSVWorkOrderItem, RSSVWorkOrderItem.inventoryID> e)
        {
            RSSVWorkOrderItem row = e.Row;

            if (row.InventoryID != null)
            {
                //Use the PXSelector attribute to select the stock items
                InventoryItem item =
                    PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(
                    e.Cache, row) as InventoryItem;
                //Copy the base price from the stock item
                row.BasePrice = item.BasePrice;
                //Retrieve the extension fields
                InventoryItemExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemExt>(item);
                if (itemExt != null)
                {
                    // Copy the repair item type from the stock item
                    row.RepairItemType = itemExt.UsrRepairItemType;

                }
            }
        }

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
                    e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row,
                    WorkOrderStatusConstants.PendingPayment);
                if (service != null && service.Prepayment == false)
                    e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row,
                    WorkOrderStatusConstants.ReadyForAssignment);
            }
            //If Hold is selected, change the status to On Hold
            if (row.Hold == true)
                e.Cache.SetValueExt<RSSVWorkOrder.status>(e.Row,
                WorkOrderStatusConstants.OnHold);
        }

        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor,
        RSSVWorkOrderLabor.quantity> e)
        {
            if (e.NewValue == null) return;
            if ((decimal)e.NewValue < 0)
            {
                //Throwing an exception to cancel the assignment of the new
                //value to the field
                throw new PXSetPropertyException(Messages.QuantityCannotBeNegative);
            }
            RSSVWorkOrderLabor line = e.Row;
            //Retrieving the default labor item related to the work order labor
            RSSVLabor labor = SelectFrom<RSSVLabor>.
                Where<RSSVLabor.deviceID.IsEqual<@P.AsInt>.
                    And<RSSVLabor.serviceID.IsEqual<@P.AsInt>>.
                    And<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>>.View.
                Select(this, line.DeviceID, line.ServiceID, line.InventoryID);
            if (labor != null && (decimal)e.NewValue < labor.Quantity)
            {
                //Correcting the LineQty value
                e.NewValue = labor.Quantity;
                //Raising the ExceptionHandling event for the Quantity field
                //to attach the exception object to the field
                e.Cache.RaiseExceptionHandling<RSSVWorkOrderLabor.quantity>(
                    line, e.NewValue,
                    new PXSetPropertyException(Messages.QuantityToSmall,
                    PXErrorLevel.Warning));
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
        #endregion
    }
}