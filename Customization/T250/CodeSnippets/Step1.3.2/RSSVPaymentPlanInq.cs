using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PhoneRepairShop.Workflows;
using PX.Objects.SO;
using System.Collections;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {
        [PXFilterable]
        public SelectFrom<RSSVWorkOrderToPay>.
            InnerJoin<ARInvoice>.On<ARInvoice.refNbr.IsEqual<
                RSSVWorkOrderToPay.invoiceNbr>>.
            Where<RSSVWorkOrderToPay.status.IsNotEqual<
                RSSVWorkOrderWorkflow.States.paid>.
                And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                    Or<RSSVWorkOrderToPay.customerID.IsEqual<
                        RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                    Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                        RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
            View.ReadOnly DetailsView;

        protected virtual IEnumerable detailsView()
        {
            foreach (PXResult<RSSVWorkOrderToPay, ARInvoice> order in
                SelectFrom<RSSVWorkOrderToPay>.InnerJoin<ARInvoice>.
                On<ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
                Where<RSSVWorkOrderToPay.status.IsNotEqual<
                    RSSVWorkOrderWorkflow.States.paid>.
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

            var sorders = SelectFrom<SOOrderShipment>.InnerJoin<ARInvoice>.
                On<ARInvoice.refNbr.IsEqual<SOOrderShipment.invoiceNbr>>.
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
                var result = new PXResult<RSSVWorkOrderToPay, ARInvoice>(
                    workOrder, invoice);
                yield return result;
            }
        }

        public PXFilter<RSSVWorkOrderToPayFilter> Filter;

        public PXCancel<RSSVWorkOrderToPayFilter> Cancel;

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        protected virtual void _(Events.FieldSelecting<RSSVWorkOrderToPay,
            RSSVWorkOrderToPay.percentPaid> e)
        {
            if (e.Row == null) return;
            if (e.Row.OrderTotal == 0) return;

            RSSVWorkOrderToPay order = e.Row;
            var invoices = SelectFrom<ARInvoice>.
                Where<ARInvoice.refNbr.IsEqual<@P.AsString>>.View.Select(
                this, order.InvoiceNbr);
            if (invoices.Count == 0)
                return;
            ARInvoice first = invoices[0];
            e.ReturnValue = (order.OrderTotal - first.CuryDocBal) /
                order.OrderTotal * 100;
        }

        public static RSSVWorkOrderToPay RSSVWorkOrderToPay
            (SOOrderShipment shipment)
        {
            RSSVWorkOrderToPay ret = new RSSVWorkOrderToPay();
            ret.OrderNbr = shipment.OrderNbr;
            ret.InvoiceNbr = shipment.InvoiceNbr;
            return ret;
        }
    }

    ...
}