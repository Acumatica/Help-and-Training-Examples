using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CT;

namespace PhoneRepairShop
{
    [PXCacheName("Warranty")]
    public class RSSVWarranty : IBqlTable
  {
    #region ContractID
    [PXDBInt(IsKey = true)]
    [PXUIField(DisplayName = "Contract ID")]
[PXDimensionSelector(ContractTemplateAttribute.DimensionName,
    typeof(Search<ContractTemplate.contractID,
    Where<ContractTemplate.baseType.IsEqual<CTPRType.contractTemplate>>>),
    typeof(ContractTemplate.contractCD),
    DescriptionField = typeof(ContractTemplate.contractCD))]
        public virtual int? ContractID { get; set; }
    public abstract class contractID : PX.Data.BQL.BqlInt.Field<contractID> { }
    #endregion

    #region DeviceID
    [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.deviceID))]
        [PXParent(
            typeof(SelectFrom<RSSVRepairPrice>.
                Where<RSSVRepairPrice.deviceID.IsEqual<
                    RSSVWarranty.deviceID.FromCurrent>.
                And<RSSVRepairPrice.serviceID.IsEqual<
                    RSSVWarranty.serviceID.FromCurrent>>>
            ))]
        public virtual int? DeviceID { get; set; }
    public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
    #endregion

    #region ServiceID
    [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.serviceID))]
        public virtual int? ServiceID { get; set; }
    public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region DefaultWarranty
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? DefaultWarranty { get; set; }
        public abstract class defaultWarranty : PX.Data.BQL.BqlBool.Field<defaultWarranty> { }
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
        public virtual string CreatedByScreenID { get; set; }
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
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region Noteid
        [PXNote]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion
    }
}