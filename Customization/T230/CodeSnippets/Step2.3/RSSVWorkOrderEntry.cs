using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
...
        #region Actions
...
        public PXAction<RSSVWorkOrder> UpdateLaborPrices;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Update Prices", Enabled = true)]
        protected virtual void updateLaborPrices()
        {
            var order = WorkOrders.Current;
            if (order.ServiceID != null && order.DeviceID != null)
            {
                var laborItems = Labor.Select();
                foreach (RSSVWorkOrderLabor labor in laborItems)
                {
                    RSSVLabor origItem = SelectFrom<RSSVLabor>.
                        Where<RSSVLabor.inventoryID.IsEqual<@P.AsInt>>.View.
                        Select(this, labor.InventoryID);
                    if (origItem != null)
                    {
                        labor.DefaultPrice = origItem.DefaultPrice;
                        Labor.Update(labor);
                    }
                }

                Actions.PressSave();
            }
        }
        #endregion

        #region Event Handlers 

...

        // Make the Assign to Me action visible only for orders with
        // the Ready for Assignment status and available if Assignee is not the current user
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e)
        {
            RSSVWorkOrder row = e.Row;
            if (row == null) return;
            AssignToMe.SetEnabled((row.Status == WorkOrderStatusConstants.ReadyForAssignment ||
              row.Status == WorkOrderStatusConstants.OnHold) &&
              WorkOrders.Cache.GetStatus(row) != PXEntryStatus.Inserted);
            AssignToMe.SetVisible(row.Assignee != PXAccess.GetContactID());

            UpdateItemPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
            UpdateLaborPrices.SetEnabled(WorkOrders.Current.InvoiceNbr == null);
        }
        #endregion
...
    }
}