using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order to Pay")]
    public class RSSVWorkOrderToPay : RSSVWorkOrder
    {
        #region InvoiceNbr
        public new abstract class invoiceNbr : PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion

        #region Status
        public new abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region OrderNbr
        public new abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region PercentPaid
        [PXDecimal]
        [PXUIField(DisplayName = "Percent Paid")]
        public virtual Decimal? PercentPaid { get; set; }
        public abstract class percentPaid : PX.Data.BQL.BqlDecimal.Field<percentPaid> { }
        #endregion

        public new abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }

        public new abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }

        #region OrderType
        [PXString]
        [PXUIField(DisplayName = "Order Type")]
        [PXUnboundDefault(OrderTypeConstants.WorkOrder)]
        [PXStringList(
          new string[]
          {
            OrderTypeConstants.SalesOrder,
            OrderTypeConstants.WorkOrder
          },
          new string[]
          {
            Messages.SalesOrder,
            Messages.WorkOrder
          })]
        public virtual String OrderType { get; set; }
        public abstract class orderType : PX.Data.BQL.BqlDecimal.Field<orderType> { }
        #endregion


    }
}
