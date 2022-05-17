namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ...

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
            PXUIFieldAttribute.SetEnabled<RSSVWorkOrder.assignTo>(
                WorkOrders.Cache, null, true);
        }

        ...
    }
}