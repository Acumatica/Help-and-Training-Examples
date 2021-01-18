using System;
using PX.Data;

namespace PhoneRepairShop
{ 
    [PXCacheName("Repair Item Included in Repair Work Order")]
    public class RSSVWorkOrderItem : IBqlTable
    {
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDBDefault(typeof(RSSVWorkOrder.orderNbr))]
        [PXParent(typeof(SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.orderNbr.IsEqual<RSSVWorkOrderItem.orderNbr.FromCurrent>>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(RSSVWorkOrder.repairItemLineCntr))]
        [PXUIField(DisplayName = "Line Nbr.", Visible = false)]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region RepairItemType
        [PXDBString(2, IsFixed = true)]
        [PXStringList(
            new string[]
            {
                RepairItemTypeConstants.Battery,
                RepairItemTypeConstants.Screen,
                RepairItemTypeConstants.ScreenCover,
                RepairItemTypeConstants.BackCover,
                RepairItemTypeConstants.Motherboard
            },
            new string[]
            {
                Messages.Battery,
                Messages.Screen,
                Messages.ScreenCover,
                Messages.BackCover,
                Messages.Motherboard
            })]
        [PXUIField(DisplayName = "Repair Item Type")]
        public virtual string RepairItemType { get; set; }
        public abstract class repairItemType : PX.Data.BQL.BqlString.Field<repairItemType>
        { }
        #endregion

        #region InventoryID
        [Inventory]
        [PXRestrictor(typeof(
            Where<InventoryItemExt.usrRepairItem.IsEqual<True>.
            And<Brackets<RSSVWorkOrderItem.repairItemType.FromCurrent.IsNull.
            Or<InventoryItemExt.usrRepairItemType.
            IsEqual<RSSVWorkOrderItem.repairItemType.FromCurrent>>>>>),
            Messages.StockItemIncorrectRepairItemType,
            typeof(RSSVWorkOrderItem.repairItemType))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        [PXFormula(null, typeof(SumCalc<RSSVWorkOrder.orderTotal>))]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice : PX.Data.BQL.BqlDecimal.Field<basePrice> { }
        #endregion
		
		// system fields
    }
}