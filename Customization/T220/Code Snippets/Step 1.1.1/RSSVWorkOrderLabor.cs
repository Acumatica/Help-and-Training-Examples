using System;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace PhoneRepairShop
{

	[PXCacheName("Work Order Labor")]
    public class RSSVWorkOrderLabor : IBqlTable
    {
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDBDefault(typeof(RSSVWorkOrder.orderNbr))]
        [PXParent(typeof(SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.orderNbr.IsEqual<RSSVWorkOrderLabor.orderNbr.FromCurrent>>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region InventoryID
        [Inventory(IsKey = true)]
        [PXRestrictor(typeof(
            Where<InventoryItem.stkItem, Equal<False>>),
            Messages.CannotAddStockItemToRepairPrice)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion


        #region DefaultPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Default Price")]
        public virtual Decimal? DefaultPrice { get; set; }
        public abstract class defaultPrice : PX.Data.BQL.BqlDecimal.Field<defaultPrice> { }
        #endregion

        #region Quantity
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Quantity")]
        public virtual Decimal? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion

        #region ExtPrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Ext. Price", Enabled = false)]
        [PXFormula(
            typeof(Mult<RSSVWorkOrderLabor.quantity, RSSVWorkOrderLabor.defaultPrice>),
            typeof(SumCalc<RSSVWorkOrder.orderTotal>))]
        public virtual Decimal? ExtPrice { get; set; }
        public abstract class extPrice : PX.Data.BQL.BqlDecimal.Field<extPrice> { }
        #endregion

		//system fields
    }
}