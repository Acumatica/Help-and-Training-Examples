using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry,
        RSSVWorkOrder>
    {
        #region Views

        //The primary view
        public SelectFrom<RSSVWorkOrder>.View WorkOrders = null!;

        //The view for the Repair Items tab
        public SelectFrom<RSSVWorkOrderItem>.
            Where<RSSVWorkOrderItem.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            RepairItems = null!;

        //The view for the Labor tab
        public SelectFrom<RSSVWorkOrderLabor>.
            Where<RSSVWorkOrderLabor.orderNbr.
                IsEqual<RSSVWorkOrder.orderNbr.FromCurrent>>.View
            Labor = null!;

        ////////// The added code
        //The view for the auto-numbering of records
        public PXSetup<RSSVSetup> AutoNumSetup = null!;
        #endregion

        #region Graph constructor
        public RSSVWorkOrderEntry()
        {
            RSSVSetup setup = AutoNumSetup.Current;
        }
        ////////// The end of added code
        #endregion

        #region Events
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
        protected void _(Events.FieldUpdated<RSSVWorkOrderItem,
            RSSVWorkOrderItem.inventoryID> e)
        {
            RSSVWorkOrderItem row = e.Row;
            if (row.InventoryID != null && row.RepairItemType == null)
            {
                //Use the PXSelector attribute to select the stock item.
                var item = PXSelectorAttribute.Select<
                    RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
                //Copy the repair item type from the stock item to the row.
                var itemExt = item?.GetExtension<InventoryItemExt>();
                if (itemExt != null) row.RepairItemType = itemExt.UsrRepairItemType;
            }
            e.Cache.SetDefaultExt<RSSVWorkOrderItem.basePrice>(e.Row);
        }

        protected void _(Events.FieldDefaulting<RSSVWorkOrderItem, RSSVWorkOrderItem.basePrice> e)
        {
            RSSVWorkOrderItem row = e.Row;
            if (row.InventoryID == null) return;
            //Use the PXSelector attribute to select the stock item.
            var item = PXSelectorAttribute.Select<RSSVWorkOrderItem.inventoryID>(e.Cache, row) as InventoryItem;
            //Retrieve the base price for the stock item.
            var curySettings = InventoryItemCurySettings.PK.Find(
                this, item?.InventoryID, Accessinfo.BaseCuryID ?? "USD");
            //Copy the base price from the stock item to the row.
            if (curySettings != null) e.NewValue = curySettings.BasePrice;
        }

        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor,
            RSSVWorkOrderLabor.quantity> e)
        {
            if (e.Row == null || e.NewValue == null) return;

            if ((decimal)e.NewValue < 0)
            {
                //Throwing an exception to cancel the assignment
                //of the new value to the field
                throw new PXSetPropertyException(e.Row,
                    Messages.QuantityCannotBeNegative);
            }

            var workOrder = WorkOrders.Current;
            if (workOrder != null)
            {
                //Retrieving the default labor item related to the work order labor
                RSSVLabor labor = SelectFrom<RSSVLabor>.
                    Where<RSSVLabor.serviceID.IsEqual<@P.AsInt>.
                        And<RSSVLabor.deviceID.IsEqual<@P.AsInt>>.
                        And<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>>
                    .View.Select(this, workOrder.ServiceID, workOrder.DeviceID,
                    e.Row.InventoryID);
                if (labor != null && (decimal)e.NewValue < labor.Quantity)
                {
                    //Correcting the LineQty value
                    e.NewValue = labor.Quantity;
                    //Raising the ExceptionHandling event for the Quantity field
                    //to attach the exception object to the field
                    e.Cache.RaiseExceptionHandling<RSSVWorkOrderLabor.quantity>(
                        e.Row, e.NewValue, new PXSetPropertyException(e.Row,
                            Messages.QuantityTooSmall, PXErrorLevel.Warning));
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
                        WorkOrders.Cache.RaiseExceptionHandling<
                            RSSVWorkOrder.priority>(row, originalRow.Priority,
                            new PXSetPropertyException(row, Messages.PriorityTooLow));

                        //Assign the proper priority
                        e.NewRow.Priority = WorkOrderPriorityConstants.Medium;
                    }
                }
            }
        }
        #endregion
    }
}