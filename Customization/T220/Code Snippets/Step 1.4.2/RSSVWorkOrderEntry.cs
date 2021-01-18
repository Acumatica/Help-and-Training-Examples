using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using System.Linq;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {
    
        #region Event Handlers
		...
        //Display an error if a required repair item is missing in a work order
        //for which a user clears the Hold check box.
        protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
        {
            // The modified data record (not in the cache yet)
            RSSVWorkOrder row = e.NewRow;
            // The data record that is stored in the cache
            RSSVWorkOrder originalRow = e.Row;
            if (!e.Cache.ObjectsEqual<RSSVWorkOrder.hold, RSSVWorkOrder.status>(row,
            originalRow))
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
        #endregion
    }
}