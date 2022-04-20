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

        public static void AssignOrders(List<RSSVWorkOrder> list, bool isMassProcess = false)
        {
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    continue;

                RSSVWorkOrder workOrder = list[i];
                try
                {
                    workOrder.Assignee = workOrder.AssignTo;
                    workOrderEntry.Clear();
                    workOrderEntry.WorkOrders.Current = workOrder;
                    //If the assignee is not specified, specify the default employee.
                    if (workOrder.Assignee == null)
                    {
                        //Retrieve the record with the default setting
                        RSSVSetup setupRecord = workOrderEntry.AutoNumSetup.Current;
                        workOrder.Assignee = setupRecord.DefaultEmployee;
                    }

                    //Update the work order in the cache.
                    workOrderEntry.WorkOrders.Update(workOrder);

                    //Modify the number of assigned orders for the employee.
                    RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                        new RSSVEmployeeWorkOrderQty();
                    employeeNbrOfOrders.UserID = workOrder.Assignee;
                    employeeNbrOfOrders.NbrOfAssignedOrders = 1;
                    workOrderEntry.Quantity.Insert(employeeNbrOfOrders);

                    // Trigger the Save action to save the changes to the database
                    workOrderEntry.Actions.PressSave();

                    //Display the message to indicate successful processing.
                    if (isMassProcess)
                    {
                        PXProcessing<RSSVWorkOrder>.SetInfo(i, string.Format(Messages.WorkOrderAssigned,
                            workOrder.OrderNbr));
                    }
                }
                catch (Exception e)
                {
                    PXProcessing<RSSVWorkOrder>.SetError(i, e);
                }
            }
        }

        public PXAction<RSSVWorkOrder> Complete;
        [PXButton]
        [PXUIField(DisplayName = "Complete")]
        protected virtual IEnumerable complete(PXAdapter adapter)
        {
            // Get the current order from the cache
            RSSVWorkOrder row = WorkOrders.Current;
            //Modify the number of assigned orders for the employee
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                new RSSVEmployeeWorkOrderQty();
            employeeNbrOfOrders.UserID = row.Assignee;
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