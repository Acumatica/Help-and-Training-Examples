using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PhoneRepairShop.Workflows;
using PX.TM;
using System.Collections.Generic;
using PX.Data.BQL;
using System.Linq;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ...

        public static void AssignOrders(List<RSSVWorkOrder> orders)
        {
            // The result set to run the report on.
            PXReportResultset assignedOrders =
                new PXReportResultset(typeof(RSSVWorkOrder));

            RSSVWorkOrderEntry graph =
                PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            foreach (RSSVWorkOrder order in orders)
            {
                try
                {
                    //Change the assignee to the value selected on the form
                    order.Assignee = order.AssignTo;
                    graph.Clear();
                    graph.AssignOrder(order, true);

                    // Add to the result set the order 
                    // that has been successfully assigned.
                    if (order.Status == WorkOrderStatusConstants.Assigned)
                    {
                        assignedOrders.Add(order);
                    }
                }
                catch (Exception e)
                {
                    PXProcessing<RSSVWorkOrder>.SetError(
                        orders.IndexOf(order), e);
                }
            }

            if (assignedOrders.GetRowCount() > 0)
            {
                throw new PXReportRequiredException(assignedOrders, "RS601000",
                                                    Messages.ReportRS601000Title);
            }
        }

        ...
    }
}