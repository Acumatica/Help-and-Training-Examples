using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Price")]
    public class RSSVRepairPrice : IBqlTable
    {
        #region PriceID
        [PXDBIdentity]
        public virtual int? PriceID { get; set; }
        public abstract class priceID : PX.Data.BQL.BqlInt.Field<priceID> { }
        #endregion

        #region DeviceID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Device", Required = true)]
        [PXSelector(
            typeof(Search<RSSVDevice.deviceID>),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.description),
            DescriptionField = typeof(RSSVDevice.description),
            SelectorMode = PXSelectorMode.DisplayModeText)]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion

        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Service", Required = true)]
        [PXSelector(
            typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            DescriptionField = typeof(RSSVRepairService.description),
            SelectorMode = PXSelectorMode.DisplayModeText)]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region Price
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Approximate Price", Enabled = false)]
        public virtual Decimal? Price { get; set; }
        public abstract class price : PX.Data.BQL.BqlDecimal.Field<price> { }
        #endregion

        #region RepairItemLineCntr
        [PXDBInt()]
        [PXDefault(0)]
        public virtual Int32? RepairItemLineCntr { get; set; }
        public abstract class repairItemLineCntr : PX.Data.BQL.BqlInt.Field<repairItemLineCntr> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion
    }
}