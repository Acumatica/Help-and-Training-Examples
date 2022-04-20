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

        public PXAction<RSSVWorkOrder> Assign;
        [PXProcessButton]
        [PXUIField(DisplayName = "Assign")]
        protected virtual IEnumerable assign(PXAdapter adapter)
        {
            bool isMassProcess = adapter.MassProcess;
            // Populate a local list variable.
            List<RSSVWorkOrder> list = new List<RSSVWorkOrder>();
            foreach (RSSVWorkOrder order in adapter.Get<RSSVWorkOrder>())
            {
                list.Add(order);
            }
            // Trigger the Save action to save changes in the database.
            Save.Press();

            PXLongOperation.StartOperation(this, delegate ()
            {
            AssignOrders(list, isMassProcess);
            });

            // Return the local list variable.
            return list;
        }

        public static void AssignOrders(List<RSSVWorkOrder> list, 
            bool isMassProcess = false)
        {
            var workOrderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    continue;

                RSSVWorkOrder workOrder = list[i];
                try
                {
                    workOrderEntry.Clear();
                    workOrderEntry.WorkOrders.Current = workOrder;
                    //If the assignee is not specified, 
                    //specify the default employee.
                    if (workOrder.Assignee == null)
                    {
                        //Retrieve the record with the default setting
                        RSSVSetup setupRecord = 
                            workOrderEntry.AutoNumSetup.Current;
                        workOrder.Assignee = setupRecord.DefaultEmployee;
                    }

                    //Update the work order in the cache.
                    workOrderEntry.WorkOrders.Update(workOrder);

                    //Trigger the Save action to save the changes 
                    //to the database
                    workOrderEntry.Actions.PressSave();

                    //Display the message to indicate successful processing.
                    if (isMassProcess)
                    {
                        PXProcessing<RSSVWorkOrder>.SetInfo(i,
                            string.Format(Messages.WorkOrderAssigned,
                            workOrder.OrderNbr));
                    }
                }
                catch (Exception e)
                {
                    PXProcessing<RSSVWorkOrder>.SetError(i, e);
                }
            }
        }

        ...
    }
}