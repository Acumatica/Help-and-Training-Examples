namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Views

        ...

        public SelectFrom<RSSVWorkOrderPayment>.
            Where<RSSVWorkOrderPayment.invoiceNbr.IsEqual<
                RSSVWorkOrder.invoiceNbr.FromCurrent>>.
            View Payments;

        #endregion

        ...
    }

    public class ARPaymentEvents : PXEntityEvent<ARPayment>.Container<ARPaymentEvents>
    {
        public PXEntityEvent<ARPayment> InvoiceGotPrepaid;
    }

    public class MyEvents : PXEntityEventBase<ARInvoice>.Container<MyEvents>
    {
        public PXEntityEvent<ARInvoice> InvoiceGotPrepaid;
    }

    public class RSSVWorkOrderEntry_Extension : PXGraphExtension<RSSVWorkOrderEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
            Base.ActionsMenuItem.AddMenuAction(Base.CreateInvoiceAction);
        }
    }
}