using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;

namespace PhoneRepairShop
{
    [PXCacheName(Messages.RSSVDevice)]
    [PXPrimaryGraph(typeof(RSSVDeviceMaint))]
    public class RSSVDevice : PXBqlTable, IBqlTable
    {
        #region DeviceID
        [PXDBIdentity]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion

        #region DeviceCD
        [PXDBString(15, IsUnicode = true, IsKey = true, InputMask = ">aaaaaaaaaaaaaaa")]
        [PXDefault]
        [PXUIField(DisplayName = "Device Code")]
        [PXSelector(typeof(Search<RSSVDevice.deviceCD>),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.active),
            typeof(RSSVDevice.avgComplexityOfRepair))]
        public virtual string? DeviceCD { get; set; }
        public abstract class deviceCD : PX.Data.BQL.BqlString.Field<deviceCD> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string? Description { get; set; }
        public abstract class description :
        PX.Data.BQL.BqlString.Field<description>
        { }
        #endregion

        #region Active
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Active")]
        public virtual bool? Active { get; set; }
        public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
        #endregion

        #region AvgComplexityOfRepair
        [PXDBString(1, IsFixed = true)]
        [PXDefault(RepairComplexity.Medium)]
        [PXUIField(DisplayName = "Complexity")]
        [PXStringList(
        new string[]
        {
            RepairComplexity.Low,
            RepairComplexity.Medium,
            RepairComplexity.High
        },
        new string[]
        {
            Messages.Low, Messages.Medium, Messages.High
        })]
        public virtual string? AvgComplexityOfRepair { get; set; }
        public abstract class avgComplexityOfRepair :
        PX.Data.BQL.BqlString.Field<avgComplexityOfRepair>
        { }
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
