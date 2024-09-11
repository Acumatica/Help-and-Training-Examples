using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    [PXCacheName(Messages.RSSVRepairItem)]
    public class RSSVRepairItem : PXBqlTable, IBqlTable
    {
        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.serviceID))]
        [PXParent(typeof(SelectFrom<RSSVRepairPrice>.
            Where<RSSVRepairPrice.serviceID.
                IsEqual<RSSVRepairItem.serviceID.FromCurrent>.
                And<RSSVRepairPrice.deviceID.
                IsEqual<RSSVRepairItem.deviceID.FromCurrent>>>))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region DeviceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.deviceID))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(RSSVRepairPrice.repairItemLineCntr))]
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
        public virtual string? RepairItemType { get; set; }
        public abstract class repairItemType :
            PX.Data.BQL.BqlString.Field<repairItemType>
        { }
        #endregion

        #region InventoryID
        [PXRestrictor(typeof(
            Where<InventoryItemExt.usrRepairItem.IsEqual<True>.
                And<Brackets<
                    RSSVRepairItem.repairItemType.FromCurrent.IsNull.
                        Or<InventoryItemExt.usrRepairItemType.
                            IsEqual<RSSVRepairItem.repairItemType.FromCurrent>>>>>),
            Messages.StockItemIncorrectRepairItemType,
            typeof(RSSVRepairItem.repairItemType))]
        [Inventory]
        [PXDefault]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID :
            PX.Data.BQL.BqlInt.Field<inventoryID>
        { }
        #endregion

        #region Required
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Required")]
        public virtual bool? Required { get; set; }
        public abstract class required :
            PX.Data.BQL.BqlBool.Field<required>
        { }
        #endregion

        #region IsDefault
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Default")]
        public virtual bool? IsDefault { get; set; }
        public abstract class isDefault :
            PX.Data.BQL.BqlBool.Field<isDefault>
        { }
        #endregion

        #region BasePrice
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Price")]
        [PXFormula(null,
            typeof(SumCalc<RSSVRepairPrice.price>))]
        public virtual Decimal? BasePrice { get; set; }
        public abstract class basePrice : 
            PX.Data.BQL.BqlDecimal.Field<basePrice> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime :
            PX.Data.BQL.BqlDateTime.Field<createdDateTime>
        { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID :
            PX.Data.BQL.BqlGuid.Field<createdByID>
        { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string? CreatedByScreenID { get; set; }
        public abstract class createdByScreenID :
            PX.Data.BQL.BqlString.Field<createdByScreenID>
        { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime :
            PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime>
        { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID :
            PX.Data.BQL.BqlGuid.Field<lastModifiedByID>
        { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string? LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID :
            PX.Data.BQL.BqlString.Field<lastModifiedByScreenID>
        { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[]? Tstamp { get; set; }
        public abstract class tstamp :
            PX.Data.BQL.BqlByteArray.Field<tstamp>
        { }
        #endregion

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}