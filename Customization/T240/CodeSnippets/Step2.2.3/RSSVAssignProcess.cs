namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ...

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate(AssignOrders);
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }

        public static void AssignOrders(List<RSSVWorkOrder> orders)
        {
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
                }
                catch (Exception e)
                {
                    PXProcessing<RSSVWorkOrder>.SetError(
                        orders.IndexOf(order), e);
                }
            }
        }

        ...
    }
}