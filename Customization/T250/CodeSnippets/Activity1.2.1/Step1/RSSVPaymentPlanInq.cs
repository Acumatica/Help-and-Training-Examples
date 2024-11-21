using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {
        [PXFilterable]
        public 
            SelectFrom<RSSVWorkOrderToPay>.
            InnerJoin<ARInvoice>.On<
                ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
            Where<RSSVWorkOrderToPay.status.IsNotEqual<RSSVWorkOrderEntry_Workflow.States.paid>>.
            View.ReadOnly DetailsView = null!;

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

    ////////// The added code
    [PXHidden]
    public class RSSVWorkOrderToPayFilter : PXBqlTable, IBqlTable
    {
        #region ServiceID
        [PXInt()]
        [PXUIField(DisplayName = "Service")]
        [PXSelector(
            typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            DescriptionField = typeof(RSSVRepairService.description),
            SelectorMode = PXSelectorMode.DisplayModeText)]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID>
        { }
        #endregion

        #region CustomerID
        [CustomerActive(DisplayName = "Customer ID")]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID :
            PX.Data.BQL.BqlInt.Field<customerID>
        { }
        #endregion
    }
    ////////// The end of added code
}