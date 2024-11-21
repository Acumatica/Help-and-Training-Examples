using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {
        ////////// The added code
        [PXFilterable]
        public SelectFrom<RSSVWorkOrderToPay>.
            InnerJoin<ARInvoice>.On<ARInvoice.refNbr.
                IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
            Where<RSSVWorkOrderToPay.status.
                IsNotEqual<RSSVWorkOrderEntry_Workflow.States.paid>>.
            View.ReadOnly DetailsView = null!;
        ////////// The end of added code

        protected virtual void _(Events.RowSelecting<RSSVWorkOrderToPay> e)
        {
            using (new PXConnectionScope())
            {
                if (e.Row == null) return;
                if (e.Row.OrderTotal == 0) return;
                RSSVWorkOrderToPay order = e.Row;
                var invoices = 
                    SelectFrom<ARInvoice>.
                    Where<ARInvoice.refNbr.IsEqual<@P.AsString>>.
                    View.Select(this, order.InvoiceNbr);
                if (invoices.Count == 0)
                    return;
                ARInvoice first = invoices[0];
                e.Row.PercentPaid = (order.OrderTotal - first.CuryDocBal) /
                    order.OrderTotal * 100;
            }
        }
    }
}