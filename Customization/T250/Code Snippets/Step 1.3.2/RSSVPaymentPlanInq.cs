using System.Collections;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.SO;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {

		...
		
		public static RSSVWorkOrderToPay RSSVWorkOrderToPay(SOOrderShipment shipment)
        {
            RSSVWorkOrderToPay ret = new RSSVWorkOrderToPay();
            ret.OrderNbr = shipment.OrderNbr;
            ret.InvoiceNbr = shipment.InvoiceNbr;
            return ret;
        }

        protected virtual IEnumerable detailsView()
        {
            foreach (PXResult<RSSVWorkOrderToPay, ARInvoice> order in SelectFrom<RSSVWorkOrderToPay>.
              InnerJoin<ARInvoice>.On<ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
              Where<RSSVWorkOrderToPay.status.IsNotEqual<workOrderStatusPaid>.
                And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                  Or<RSSVWorkOrderToPay.customerID.IsEqual<
                    RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                  And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                    Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                      RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
            View.Select(this))
            {
                yield return order;
            }

            var sorders = SelectFrom<SOOrderShipment>.
              InnerJoin<ARInvoice>.On<ARInvoice.refNbr.IsEqual<SOOrderShipment.invoiceNbr>>.
              Where<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                Or<SOOrderShipment.customerID.IsEqual<
                  RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
              View.Select(this);
            foreach (PXResult<SOOrderShipment, ARInvoice> order in sorders)
            {
                SOOrderShipment soshipment = order;
                ARInvoice invoice = order;
                RSSVWorkOrderToPay workOrder = RSSVWorkOrderToPay(soshipment);
                workOrder.OrderType = OrderTypeConstants.SalesOrder;
                var result = new PXResult<RSSVWorkOrderToPay, ARInvoice>(workOrder, invoice);
                yield return result;
            }
        }

		...


    }
}