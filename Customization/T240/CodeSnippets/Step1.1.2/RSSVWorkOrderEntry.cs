using System;
using System.Collections;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        ...

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

            // Trigger the Save action to save the changes to the database
            Actions.PressSave();
            //Display the message to indicate successful processing.
            if (isMassProcess)
            {
                PXProcessing.SetInfo(string.Format(Messages.WorkOrderAssigned,
                    order.OrderNbr));
            }
        }

        ...
    }
}