using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;
using PX.Objects.CS;

namespace PhoneRepairShop
{
    public class RSSVSetup : IBqlTable
    {
        #region WalkInCustomerID
        [CustomerActive(DisplayName = "Walk-In Customer", DescriptionField = typeof(Customer.acctName))]
        [PXDefault]
        public virtual int? WalkInCustomerID { get; set; }
        public abstract class walkInCustomerID : PX.Data.BQL.BqlInt.Field<walkInCustomerID> { }
        #endregion

        #region DefaultEmployee
        [Owner(DisplayName = "Default Employee")]
        [PXDefault]
        public virtual int? DefaultEmployee { get; set; }
        public abstract class defaultEmployee : PX.Data.BQL.BqlInt.Field<defaultEmployee> { }
        #endregion


        #region PrepaymentPercent
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Prepayment Percent", Required = true)]
        public virtual Decimal? PrepaymentPercent { get; set; }
        public abstract class prepaymentPercent :
        PX.Data.BQL.BqlDecimal.Field<prepaymentPercent>
        { }
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
