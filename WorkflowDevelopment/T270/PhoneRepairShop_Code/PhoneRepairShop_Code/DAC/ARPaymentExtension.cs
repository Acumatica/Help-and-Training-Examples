using PX.Data;
using PX.Objects.AR;
using System;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod 
    // extension should be constantly active
    public sealed class ARPaymentExt : PXCacheExtension<ARPayment>
    {
        #region PrepaymentPercent
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Prepayment Percent")]
        public Decimal? UsrPrepaymentPercent { get; set; }
        public abstract class usrPrepaymentPercent :
            PX.Data.BQL.BqlDecimal.Field<usrPrepaymentPercent>
        { }
        #endregion
    }
}
