using ARCashSale = PX.Objects.AR.Standalone.ARCashSale;
using CRLocation = PX.Objects.CR.Standalone.Location;
using IRegister = PX.Objects.CM.IRegister;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.AR.BQL;
using PX.Objects.AR;
using PX.Objects.CM.Extensions;
using PX.Objects.Common.Abstractions;
using PX.Objects.Common.Attributes;
using PX.Objects.Common.MigrationMode;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL.DAC;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.GL;
using PX.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

namespace PX.Objects.AR
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class ARPaymentExt : PXCacheExtension<PX.Objects.AR.ARPayment>
    {
        #region PrepaymentPercent
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Prepayment Percent")]
        public Decimal? UsrPrepaymentPercent { get; set; }
        public abstract class usrPrepaymentPercent : PX.Data.BQL.BqlDecimal.Field<usrPrepaymentPercent> { }
        #endregion
    }
}