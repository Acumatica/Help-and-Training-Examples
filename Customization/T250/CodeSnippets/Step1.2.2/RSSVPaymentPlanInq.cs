using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {

        #region Data Views

        //[PXFilterable]
        //public SelectFrom<RSSVWorkOrderToPay>.
        //    InnerJoin<ARInvoice>.On<ARInvoice.refNbr.
        //    IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
        //    Where<RSSVWorkOrderToPay.status.IsNotEqual<workOrderStatusPaid>>.
        //    View.ReadOnly DetailsView;

        [PXFilterable]
        public SelectFrom<RSSVWorkOrderToPay>.InnerJoin<ARInvoice>.
            On<ARInvoice.refNbr.IsEqual<RSSVWorkOrderToPay.invoiceNbr>>.
            Where<RSSVWorkOrderToPay.status.IsNotEqual<workOrderStatusPaid>.
            And<RSSVWorkOrderToPayFilter.customerID.FromCurrent.IsNull.
              Or<RSSVWorkOrderToPay.customerID.IsEqual<
                RSSVWorkOrderToPayFilter.customerID.FromCurrent>>>.
              And<RSSVWorkOrderToPayFilter.serviceID.FromCurrent.IsNull.
              Or<RSSVWorkOrderToPay.serviceID.IsEqual<
                RSSVWorkOrderToPayFilter.serviceID.FromCurrent>>>>.
            View.ReadOnly DetailsView;

        public PXFilter<RSSVWorkOrderToPayFilter> Filter;

        #endregion

        #region Actions 

        public PXCancel<RSSVWorkOrderToPayFilter> Cancel;

        #endregion

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