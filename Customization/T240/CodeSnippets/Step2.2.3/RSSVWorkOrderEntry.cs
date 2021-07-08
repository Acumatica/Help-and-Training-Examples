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
        #region Views

        ...

        //The view for the calculation of the number of assigned work orders 
        //per employee
        public SelectFrom<RSSVEmployeeWorkOrderQty>.View Quantity;

        #endregion

        ...

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
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = order.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = 1;
            Quantity.Insert(employeeNbrOfOrders);

            // Trigger the Save action to save the changes to the database
            Actions.PressSave();
            //Display the message to indicate successful processing.
            if (isMassProcess)
            {
                PXProcessing.SetInfo(string.Format(Messages.WorkOrderAssigned,
                    order.OrderNbr));
            }
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Complete", Enabled = false)]
        protected virtual IEnumerable complete(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.Userid = row.Assignee;
            employeeNbrOfOrders.NbrOfAssignedOrders = -1;
            Quantity.Insert(employeeNbrOfOrders);
            // Trigger the Save action to save changes in the database
            Actions.PressSave();
            return adapter.Get();
        }

        ...
    }

    ...
}