using PX.Data;
using PX.Objects.AR;
using System;

namespace PhoneRepairShop
{
    public sealed class ARPaymentExt : PXCacheExtension<ARPayment>
    {
        #region PrepaymentPercent
        [PXDBDecimal()]
        //[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXDefault(typeof(Select<RSSVSetup>), SourceField = typeof(RSSVSetup.prepaymentPercent),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Prepayment Percent")]
        public decimal? UsrPrepaymentPercent { get; set; }
        public abstract class usrPrepaymentPercent : PX.Data.BQL.BqlDecimal.Field<usrPrepaymentPercent> { }
        #endregion
    }
}