namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXFilter<RSSVWorkOrderToAssignFilter> Filter;
        public PXCancel<RSSVWorkOrderToAssignFilter> Cancel;
        public PXFilteredProcessing<RSSVWorkOrderToAssign, 
            RSSVWorkOrderToAssignFilter,
            Where<RSSVWorkOrderToAssign.status.IsEqual<
                RSSVWorkOrderWorkflow.States.readyForAssignment>.
                And<RSSVWorkOrderToAssign.timeWithoutAction.IsGreaterEqual<
                    RSSVWorkOrderToAssignFilter.timeWithoutAction.
                        FromCurrent>.
                And<RSSVWorkOrderToAssign.priority.IsEqual<
                    RSSVWorkOrderToAssignFilter.priority.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.priority.FromCurrent.
                        IsNull>>.
                And<RSSVWorkOrderToAssign.serviceID.IsEqual<
                    RSSVWorkOrderToAssignFilter.serviceID.FromCurrent>.
                    Or<RSSVWorkOrderToAssignFilter.serviceID.FromCurrent.
                        IsNull>>>>,
            OrderBy<Desc<RSSVWorkOrderToAssign.timeWithoutAction, 
                RSSVWorkOrderToAssign.priority.Desc>>> WorkOrders;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            WorkOrders.SetProcessDelegate<RSSVWorkOrderEntry>(
                delegate (RSSVWorkOrderEntry graph, RSSVWorkOrderToAssign order)
                {
                    try
                    {
                        graph.Clear();
                        graph.AssignOrder(order, true);
                    }
                    catch (Exception e)
                    {
                        PXProcessing<RSSVWorkOrderToAssign>.SetError(e);
                    }
                });
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrderToAssign.assignee>(
                WorkOrders.Cache, null, true);
        }

        ...

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        ...
    }
}