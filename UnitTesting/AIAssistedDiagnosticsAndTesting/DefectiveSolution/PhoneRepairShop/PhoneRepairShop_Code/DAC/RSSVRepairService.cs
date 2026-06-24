using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXCacheName(Messages.RSSVRepairService)]
    public class RSSVRepairService : PXBqlTable, IBqlTable
    {
        #region ServiceID
        [PXDBIdentity]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region ServiceCD
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">aaaaaaaaaaaaaaa")]
        [PXDefault]
        [PXUIField(DisplayName = "Service ID")]
        public virtual string? ServiceCD { get; set; }
        public abstract class serviceCD : PX.Data.BQL.BqlString.Field<serviceCD> { }
        #endregion

        #region Description
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [PXUIField(DisplayName = "Description")]
        public virtual string? Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Active
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Active")]
        public virtual bool? Active { get; set; }
        public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
        #endregion

        #region WalkInService
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Walk-In Service")]
        public virtual bool? WalkInService { get; set; }
        public abstract class walkInService : PX.Data.BQL.BqlBool.Field<walkInService> { }
        #endregion

        #region PreliminaryCheck
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Requires Preliminary Check")]
        public virtual bool? PreliminaryCheck { get; set; }
        public abstract class preliminaryCheck : PX.Data.BQL.BqlBool.Field<preliminaryCheck> { }
        #endregion

        #region Prepayment
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Requires Prepayment")]
        public virtual bool? Prepayment { get; set; }
        public abstract class prepayment : PX.Data.BQL.BqlBool.Field<prepayment> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string? CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string? LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[]? Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}