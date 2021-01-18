using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {
    
        #region Event Handlers
		...
        //Validate that Quantity is greater than or equal to 0 and
        //correct the value to the default if the value is less than the default.
        protected virtual void _(Events.FieldVerifying<RSSVWorkOrderLabor, RSSVWorkOrderLabor.quantity> e)
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
                .View.Select(this, workOrder.ServiceID, workOrder.DeviceID,
                e.Row.InventoryID);
                if (labor != null && (decimal)e.NewValue < labor.Quantity)
                {
                    //Correcting the LineQty value
                    e.NewValue = labor.Quantity;
                    //Raising the ExceptionHandling event for the Quantity field
                    //to attach the exception object to the field
                    e.Cache.RaiseExceptionHandling<RSSVWorkOrderLabor.quantity>(e.Row,
                    e.NewValue,
                    new PXSetPropertyException(Messages.QuantityToSmall,
                    PXErrorLevel.Warning));
                }
            }
        }
        #endregion
    }
}