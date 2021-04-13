using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    [PXCacheName("Invoice and Payment of the Repair Work Order")]
    [PXProjection(typeof(SelectFrom<PX.Objects.AR.ARInvoice>.
        InnerJoin<ARAdjust>.On<ARAdjust.adjdRefNbr.IsEqual<ARInvoice.refNbr>.
        And<ARAdjust.adjdDocType.IsEqual<ARInvoice.docType>>>.
        AggregateTo<Max<ARAdjust.adjgDocDate>, GroupBy<ARAdjust.adjdRefNbr>,
            GroupBy<ARAdjust.adjdDocType>>))]
    public class RSSVWorkOrderPayment : IBqlTable
    {
        #region InvoiceNbr
        [PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "",
          BqlField = typeof(ARInvoice.refNbr))]
        [PXUIField(DisplayName = "Invoice Nbr.", Enabled = false)]
        public virtual String InvoiceNbr { get; set; }
        public abstract class invoiceNbr :
            PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion

        #region DueDate
        [PXDBDate(BqlField = typeof(PX.Objects.AR.ARInvoice.dueDate))]
        [PXUIField(DisplayName = "Due Date", Enabled = false)]
        public virtual DateTime? DueDate { get; set; }
        public abstract class dueDate :
            PX.Data.BQL.BqlDateTime.Field<dueDate> { }
        #endregion

        #region AdjgRefNbr
        [PXDBString(BqlField = typeof(ARAdjust.adjgRefNbr))]
        [PXUIField(DisplayName = "Latest Payment", Enabled = false)]
        public virtual String AdjgRefNbr { get; set; }
        public abstract class adjgRefNbr :
            PX.Data.BQL.BqlString.Field<adjgRefNbr> { }
        #endregion

        #region CuryAdjdAmt
        [PXDBDecimal(BqlField = typeof(ARAdjust.curyAdjdAmt))]
        [PXUIField(DisplayName = "Latest Amount Paid", Enabled = false)]
        public virtual Decimal? CuryAdjdAmt { get; set; }
        public abstract class curyAdjdAmt :
            PX.Data.BQL.BqlDecimal.Field<curyAdjdAmt> { }
        #endregion
    }
}
