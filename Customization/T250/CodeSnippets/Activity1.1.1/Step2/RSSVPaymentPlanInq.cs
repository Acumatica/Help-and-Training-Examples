using System;
using PX.Data;
////////// The added code
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.AR;
////////// The end of added code

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {
        public PXFilter<MasterTable> MasterView;
        public PXFilter<DetailsTable> DetailsView = null!;

        [Serializable]
        public class MasterTable : PXBqlTable, IBqlTable
        {

        }

        [Serializable]
        public class DetailsTable : PXBqlTable, IBqlTable
        {

        }

        ////////// The added code
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
        ////////// The end of added code
    }
}