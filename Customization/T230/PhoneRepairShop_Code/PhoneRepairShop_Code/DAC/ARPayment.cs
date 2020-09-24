using ARCashSale = PX.Objects.AR.Standalone.ARCashSale;
using CRLocation = PX.Objects.CR.Standalone.Location;
using IRegister = PX.Objects.CM.IRegister;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AR.BQL;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.Common.Abstractions;
using PX.Objects.Common.MigrationMode;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using PhoneRepairShop;

namespace PX.Objects.AR
{
    /// <exclude/>
    public sealed class ARPaymentExt : PXCacheExtension<PX.Objects.AR.ARPayment>
    {
        #region PrepaymentPercent
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck =
          PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Prepayment Percent")]
        //[PXDefault(typeof(Select<RSSVSetup>), PersistingCheck = PXPersistingCheck.Nothing)]
        public Decimal? UsrPrepaymentPercent { get; set; }
        public abstract class usrPrepaymentPercent :
          PX.Data.BQL.BqlDecimal.Field<usrPrepaymentPercent> { }
        #endregion
    }
}