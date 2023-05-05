using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
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

        ////////// The added code
        protected virtual IEnumerable detailsView()
        {
            BqlCommand query;
            var filter = Filter.Current;
            if (filter.GroupByStatus != true)
            {
                query = new SelectFrom<RSSVWorkOrderToPay>.InnerJoin<ARInvoice>.
                    On<ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
                  Where<RSSVWorkOrderToPay.status.IsNotEqual<
                      RSSVWorkOrderWorkflow.States.paid>.
                    And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                      Or<RSSVWorkOrderToPay.customerID.IsEqual<
                        RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                      And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                        Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                          RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>();
            }
            else
            {
                query = new SelectFrom<RSSVWorkOrderToPay>.InnerJoin<ARInvoice>.
                    On<ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
                  Where<RSSVWorkOrderToPay.status.IsNotEqual<
                      RSSVWorkOrderWorkflow.States.paid>.
                    And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
                      Or<RSSVWorkOrderToPay.customerID.IsEqual<
                        RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
                      And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
                        Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                          RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
                    AggregateTo<GroupBy<RSSVWorkOrderToPay.status>,
               Sum<ARInvoice.curyDocBal>>();
            }
            var view = new PXView(this, true, query);
            foreach (PXResult<RSSVWorkOrderToPay, ARInvoice> order in
                view.SelectMulti(null))
            {
                if (filter.GroupByStatus == true)
                {
                    ((RSSVWorkOrderToPay)order[0]).OrderNbr = "";
                    ((RSSVWorkOrderToPay)order[0]).PercentPaid = null;
                    ((RSSVWorkOrderToPay)order[0]).InvoiceNbr = "";
                    ((ARInvoice)order[1]).DueDate = null;
                }
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
                RSSVWorkOrderToPay workOrder = ToRSSVWorkOrderToPay(soshipment);
                workOrder.OrderType = OrderTypeConstants.SalesOrder;
                var result = new PXResult<RSSVWorkOrderToPay, ARInvoice>(
                    workOrder, invoice);
                yield return result;
            }
        }
        ////////// The end of added code

        public PXFilter<RSSVWorkOrderToPayFilter> Filter;

        public PXCancel<RSSVWorkOrderToPayFilter> Cancel;

        public override bool IsDirty => false;

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

        public static RSSVWorkOrderToPay ToRSSVWorkOrderToPay
            (SOOrderShipment shipment) =>
        new RSSVWorkOrderToPay
            {
                OrderNbr = shipment.OrderNbr,
                InvoiceNbr = shipment.InvoiceNbr
            };
    }

    [PXHidden]
    public class RSSVWorkOrderToPayFilter : IBqlTable
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

        #region GroupByStatus
        [PXBool]
        [PXUIField(DisplayName = "Show Unpaid Subtotals")]
        public bool? GroupByStatus { get; set; }
        public abstract class groupByStatus :
            PX.Data.BQL.BqlBool.Field<groupByStatus>
        { }
        #endregion
    }
}