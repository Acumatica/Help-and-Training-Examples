namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {
        ...

        public PXFilter<RSSVWorkOrderToPayFilter> Filter;

        public PXCancel<RSSVWorkOrderToPayFilter> Cancel;

        public PXAction<RSSVWorkOrderToPay> ViewOrder;
        [PXButton]
        [PXUIField]
        protected virtual void viewOrder()
        {
            RSSVWorkOrderToPay order = DetailsView.Current;
            // if this is a repair work order
            if (order.OrderType == OrderTypeConstants.WorkOrder)
            {
                // create a new instance of the graph
                var graph = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
                // set the current property of the graph
                graph.WorkOrders.Current = graph.WorkOrders.
                  Search<RSSVWorkOrder.orderNbr>(order.OrderNbr);
                // if the order is found by its ID,
                // throw an exception to open the order in a new tab
                if (graph.WorkOrders.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true,
                      "Repair Work Order Details");
                }
            }
            // if this is a sales order
            else
            {
                // create a new instance of the graph
                var graph = PXGraph.CreateInstance<SOOrderEntry>();
                // set the current property of the graph
                graph.Document.Current = graph.Document.
                  Search<RSSVWorkOrder.orderNbr>(order.OrderNbr);
                // if the order is found by its ID,
                // throw an exception to open the order in a new tab
                if (graph.Document.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true,
                      "Sales Order Details");
                }
            }
        }

        ...
    }

    ...
}