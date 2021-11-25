using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
    public sealed class ARPaymentExt : PXCacheExtension<PX.Objects.AR.ARPayment>
    {
        //#region PrepaymentPercent
        //[PXDBDecimal()]
        //[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = "Prepayment Percent")]
        //public Decimal? UsrPrepaymentPercent { get; set; }
        //public abstract class usrPrepaymentPercent : PX.Data.BQL.BqlDecimal.Field<usrPrepaymentPercent> { }
        //#endregion
    }
}
