using System;
using PX.Data;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {   ...
       #region Event Handlers
        //Copy repair items and labor items from the Services and Prices form.
        protected virtual void _(Events.RowUpdated<RSSVWorkOrder> e)
        {
            if (WorkOrders.Cache.GetStatus(e.Row) == PXEntryStatus.Inserted &&
            !e.Cache.ObjectsEqual<RSSVWorkOrder.serviceID, RSSVWorkOrder.deviceID>(e.Row,
            e.OldRow))
            {
                if (e.Row.ServiceID != null && e.Row.DeviceID != null &&
                !IsCopyPasteContext && RepairItems.Select().Count == 0 &&
                Labor.Select().Count == 0)
                {
                    //Retrieve the default repair items
                    var repairItems = SelectFrom<RSSVRepairItem>.
                    Where<RSSVRepairItem.serviceID.IsEqual<RSSVWorkOrder.serviceID.FromCurrent>.
                    And<RSSVRepairItem.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>.View.Select(this);
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
                    And<RSSVLabor.deviceID.IsEqual<RSSVWorkOrder.deviceID.FromCurrent>>>.View.Select(this);
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
        #endregion
    }
}
