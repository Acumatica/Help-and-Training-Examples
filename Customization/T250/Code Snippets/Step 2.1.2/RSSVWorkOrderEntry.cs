...

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Data Views

		...

        //The view for the Payment Info tab
        public SelectFrom<RSSVWorkOrderPayment>.
            Where<RSSVWorkOrderPayment.invoiceNbr.IsEqual<
                RSSVWorkOrder.invoiceNbr.FromCurrent>>.
            View Payments;

        #endregion
		
	}
	
}