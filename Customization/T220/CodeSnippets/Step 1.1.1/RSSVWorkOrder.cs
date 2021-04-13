using System;
using PX.Data;

namespace PhoneRepairShop
{
  [Serializable]
  [PXCacheName("RSSVWorkOrder")]
  public class RSSVWorkOrder : IBqlTable
  {
    #region OrderNbr
    [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Order Nbr")]
    public virtual string OrderNbr { get; set; }
    public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
    #endregion

    #region CustomerID
    [PXDBInt()]
    [PXUIField(DisplayName = "Customer ID")]
    public virtual int? CustomerID { get; set; }
    public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
    #endregion

    #region DateCreated
    [PXDBDate()]
    [PXUIField(DisplayName = "Date Created")]
    public virtual DateTime? DateCreated { get; set; }
    public abstract class dateCreated : PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
    #endregion

    #region DateCompleted
    [PXDBDate()]
    [PXUIField(DisplayName = "Date Completed")]
    public virtual DateTime? DateCompleted { get; set; }
    public abstract class dateCompleted : PX.Data.BQL.BqlDateTime.Field<dateCompleted> { }
    #endregion

    #region Status
    [PXDBString(2, IsFixed = true, InputMask = "")]
    [PXUIField(DisplayName = "Status")]
    public virtual string Status { get; set; }
    public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
    #endregion

    #region Hold
    [PXDBBool()]
    [PXUIField(DisplayName = "Hold")]
    public virtual bool? Hold { get; set; }
    public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
    #endregion

    #region Description
    [PXDBString(60, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Description")]
    public virtual string Description { get; set; }
    public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
    #endregion

    #region ServiceID
    [PXDBInt()]
    [PXUIField(DisplayName = "Service ID")]
    public virtual int? ServiceID { get; set; }
    public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
    #endregion

    #region DeviceID
    [PXDBInt()]
    [PXUIField(DisplayName = "Device ID")]
    public virtual int? DeviceID { get; set; }
    public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
    #endregion

    #region OrderTotal
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Order Total")]
    public virtual Decimal? OrderTotal { get; set; }
    public abstract class orderTotal : PX.Data.BQL.BqlDecimal.Field<orderTotal> { }
    #endregion

    #region RepairItemLineCntr
    [PXDBInt()]
    [PXUIField(DisplayName = "Repair Item Line Cntr")]
    public virtual int? RepairItemLineCntr { get; set; }
    public abstract class repairItemLineCntr : PX.Data.BQL.BqlInt.Field<repairItemLineCntr> { }
    #endregion

    #region Assignee
    [PXDBInt()]
    [PXUIField(DisplayName = "Assignee")]
    public virtual int? Assignee { get; set; }
    public abstract class assignee : PX.Data.BQL.BqlInt.Field<assignee> { }
    #endregion

    #region Priority
    [PXDBString(1, IsFixed = true, InputMask = "")]
    [PXUIField(DisplayName = "Priority")]
    public virtual string Priority { get; set; }
    public abstract class priority : PX.Data.BQL.BqlString.Field<priority> { }
    #endregion

    #region InvoiceNbr
    [PXDBString(15, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Invoice Nbr")]
    public virtual string InvoiceNbr { get; set; }
    public abstract class invoiceNbr : PX.Data.BQL.BqlString.Field<invoiceNbr> { }
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
    [PXUIField(DisplayName = "Tstamp")]
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