using PX.Data;
using System;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order to Pay")]
    public class RSSVWorkOrderToPay : RSSVWorkOrder
    {
        #region InvoiceNbr
        public new abstract class invoiceNbr :
         PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion

        #region Status
        public new abstract class status :
         PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region OrderNbr
        public new abstract class orderNbr :
         PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region PercentPaid
        [PXDecimal]
        [PXUIField(DisplayName = "Percent Paid")]
        public virtual Decimal? PercentPaid { get; set; }
        public abstract class percentPaid :
         PX.Data.BQL.BqlDecimal.Field<percentPaid> { }
        #endregion
    }
}
