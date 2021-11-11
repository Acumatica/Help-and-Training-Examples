using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.SO;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        ...
        #region Event Handlers 

        ...
        // Make the Assign to Me action visible only for orders with
        // the Ready for Assignment status and available if Assignee is not the current user
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            ...

            CreateInvoiceAction.SetVisible(
                WorkOrders.Current.Status == WorkOrderStatusConstants.Completed);
            CreateInvoiceAction.SetEnabled(WorkOrders.Current.InvoiceNbr == null &&
                WorkOrders.Current.Status == WorkOrderStatusConstants.Completed);
        }
        #endregion
        ...
    }
}