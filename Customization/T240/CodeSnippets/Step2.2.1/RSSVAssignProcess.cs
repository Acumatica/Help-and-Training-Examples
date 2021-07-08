namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;
        public PXFilteredProcessing<RSSVWorkOrder, RSSVWorkOrderToAssignFilter,
            Where<RSSVWorkOrder.status.IsEqual<
                RSSVWorkOrderWorkflow.States.readyForAssignment>.
                And<RSSVWorkOrder.timeWithoutAction.IsGreaterEqual<
                    RSSVWorkOrderToAssignFilter.timeWithoutAction.FromCurrent>.
                    And<RSSVWorkOrder.priority.IsEqual<
                        RSSVWorkOrderToAssignFilter.priority.FromCurrent>.
                        Or<RSSVWorkOrderToAssignFilter.priority.FromCurrent.
                            IsNull>>.
                    And<RSSVWorkOrder.serviceID.IsEqual<
                        RSSVWorkOrderToAssignFilter.serviceID.FromCurrent>.
                        Or<RSSVWorkOrderToAssignFilter.serviceID.FromCurrent.
                            IsNull>>>>,
            OrderBy<Desc<RSSVWorkOrder.timeWithoutAction,
                RSSVWorkOrder.priority.Desc>>> WorkOrders;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate<RSSVWorkOrderEntry>(
                delegate (RSSVWorkOrderEntry graph, RSSVWorkOrder order)
                {
                    try
                    {
                        graph.Clear();
                        graph.AssignOrder(order, true);
                    }
                    catch (Exception e)
                    {
                        PXProcessing<RSSVWorkOrder>.SetError(e);
                    }
                });
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }

        ...
    }
}