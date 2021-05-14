using System;
using PX.Data;
using PX.Objects.IN;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Item")]
    public class RSSVRepairItem : IBqlTable
  {
        ...

        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        [PXFormula(null,
            typeof(SumCalc<RSSVRepairPrice.price>))]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice : PX.Data.BQL.BqlDecimal.Field<basePrice> { }
        #endregion

        ...
    }
}