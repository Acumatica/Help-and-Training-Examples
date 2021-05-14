using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Labor")]
    public class RSSVLabor : IBqlTable
    {
        ...

        #region ExtPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Ext. Price", Enabled = false)]
        [PXFormula(
            typeof(Mult<RSSVLabor.quantity, RSSVLabor.defaultPrice>),
            typeof(SumCalc<RSSVRepairPrice.price>))]
        public virtual Decimal? ExtPrice { get; set; }
        public abstract class extPrice : PX.Data.BQL.BqlDecimal.Field<extPrice> { }
        #endregion

        ...
    }
}