using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CT;

namespace PhoneRepairShop
{
    [PXCacheName(Messages.RSSVWarranty)]
    public class RSSVWarranty : PXBqlTable, IBqlTable
    {
        #region ServiceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.serviceID))]
        [PXParent(typeof(SelectFrom<RSSVRepairPrice>.
            Where<RSSVRepairPrice.deviceID.IsEqual<RSSVWarranty.deviceID.FromCurrent>.
                And<RSSVRepairPrice.serviceID.IsEqual<RSSVWarranty.serviceID.FromCurrent>>>))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region DeviceID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(RSSVRepairPrice.deviceID))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion

        #region ContractID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Contract ID")]
        [PXDimensionSelector(ContractTemplateAttribute.DimensionName,
            typeof(Search<ContractTemplate.contractID,
                Where<ContractTemplate.baseType.IsEqual<
                    CTPRType.contractTemplate>>>),
                typeof(ContractTemplate.contractCD),
            DescriptionField = typeof(ContractTemplate.description))]
        public virtual int? ContractID { get; set; }
        public abstract class contractID : PX.Data.BQL.BqlInt.Field<contractID> { }
        #endregion

        #region DefaultWarranty
        [PXDBBool()]
        [PXUIField(DisplayName = "Default Warranty")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? DefaultWarranty { get; set; }
        public abstract class defaultWarranty :
            PX.Data.BQL.BqlBool.Field<defaultWarranty>
        { }
        #endregion

        #region ContractDuration
        [PXInt(MinValue = 1, MaxValue = 1000)]
        [PXUIField(DisplayName = "Duration", Enabled = false)]
        [PXFormula(typeof(ContractTemplate.duration.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual int? ContractDuration { get; set; }
        public abstract class contractDuration : PX.Data.BQL.BqlInt.Field<contractDuration> { }
        #endregion

        #region ContractDurationType
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Duration Unit", Enabled = false)]
        [Contract.durationType.List]
        [PXFormula(typeof(ContractTemplate.durationType.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual string? ContractDurationType { get; set; }
        public abstract class contractDurationType : PX.Data.BQL.BqlString.Field<contractDurationType> { }
        #endregion

        #region ContractType
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Contract Type", Enabled = false)]
        [Contract.type.List]
        [PXFormula(typeof(ContractTemplate.type.FromSelectorOf<RSSVWarranty.contractID>))]
        public virtual string? ContractType { get; set; }
        public abstract class contractType : PX.Data.BQL.BqlString.Field<contractType> { }
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
        public virtual string? LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
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