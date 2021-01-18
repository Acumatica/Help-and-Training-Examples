using System;
using PX.Data;
using PX.Objects.IN;

namespace PhoneRepairShop
{
  public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
  {	...
	#region Event Handlers
	...
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
                RSSVRepairService service =
                PXSelectorAttribute.Select<RSSVWorkOrder.serviceID>(
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

        #endregion
    }
}