using System;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.CR;
using PX.TM;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        #endregion

        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Order Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
        [AutoNumber(typeof(RSSVSetup.numberingID), typeof(RSSVWorkOrder.dateCreated))]
        [PXSelector(typeof(Search<RSSVWorkOrder.orderNbr>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region CustomerID
        [PXDefault]
        [CustomerActive(DisplayName = "Customer ID", DescriptionField = typeof(Customer.acctName))]
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion

        #region DateCreated
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Date Created")]
        public virtual DateTime? DateCreated { get; set; }
        public abstract class dateCreated : PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
        #endregion

        #region DateCompleted
        [PXDBDate()]
        [PXUIField(DisplayName = "Date Completed", Enabled = false)]
        public virtual DateTime? DateCompleted { get; set; }
        public abstract class dateCompleted : PX.Data.BQL.BqlDateTime.Field<dateCompleted> { }
        #endregion

        #region Status
        [PXDBString(2, IsFixed = true)]
        [PXDefault(WorkOrderStatusConstants.OnHold)]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [PXStringList(
            new string[]
            {
                WorkOrderStatusConstants.OnHold,
                WorkOrderStatusConstants.PendingPayment,
                WorkOrderStatusConstants.ReadyForAssignment,
                WorkOrderStatusConstants.Assigned,
                WorkOrderStatusConstants.Completed,
                WorkOrderStatusConstants.Paid
            },
            new string[]
            {
                Messages.OnHold,
                Messages.PendingPayment,
                Messages.ReadyForAssignment,
                Messages.Assigned,
                Messages.Completed,
                Messages.Paid
            })]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Hold")]
        [PXUIVisible(typeof(Where<RSSVWorkOrder.status.IsEqual<workOrderStatusOnHold>.
            Or<RSSVWorkOrder.status.IsEqual<workOrderStatusPendingPayment>>.
            Or<RSSVWorkOrder.status.IsEqual<workOrderStatusReadyForAssignment>>>))]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region Description
        [PXDBString(60, IsUnicode = true)]
        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region ServiceID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Service",
            Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            SubstituteKey = typeof(RSSVRepairService.serviceCD),
            DescriptionField = typeof(RSSVRepairService.description))]
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region DeviceID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Device",
            Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVDevice.deviceID>),
            typeof(RSSVDevice.deviceCD),
            typeof(RSSVDevice.description),
            SubstituteKey = typeof(RSSVDevice.deviceCD),
            DescriptionField = typeof(RSSVDevice.description))]
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
        #endregion

        #region OrderTotal
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Order Total", Enabled = false)]
        public virtual Decimal? OrderTotal { get; set; }
        public abstract class orderTotal : PX.Data.BQL.BqlDecimal.Field<orderTotal> { }
        #endregion

        #region RepairItemLineCntr
        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? RepairItemLineCntr { get; set; }
        public abstract class repairItemLineCntr : PX.Data.BQL.BqlInt.Field<repairItemLineCntr> { }
        #endregion

        #region Assignee
        [Owner(typeof(Contact.workgroupID), DisplayName = "Assignee")]
        public virtual int? Assignee { get; set; }
        public abstract class assignee : PX.Data.BQL.BqlInt.Field<assignee> { }
        #endregion

        #region Priority
        [PXDBString(1, IsFixed = true)]
        [PXDefault(WorkOrderPriorityConstants.Medium)]
        [PXUIField(DisplayName = "Priority")]
        [PXStringList(
            new string[]
            {
                WorkOrderPriorityConstants.High,
                WorkOrderPriorityConstants.Medium,
                WorkOrderPriorityConstants.Low
            },
            new string[]
            {
                Messages.High,
                Messages.Medium,
                Messages.Low
            })]
        public virtual string Priority { get; set; }
        public abstract class priority : PX.Data.BQL.BqlString.Field<priority> { }
        #endregion

        #region InvoiceNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Invoice Nbr.", Enabled = false)]
        [PXSelector(typeof(SearchFor<SOInvoice.refNbr>.Where<SOInvoice.docType.IsEqual<ARDocType.invoice>>))]
        public virtual string InvoiceNbr { get; set; }
        public abstract class invoiceNbr : PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion

        #region TimeWithoutAction
        [PXInt]
        [PXDBCalced(typeof(RSSVWorkOrder.dateCreated.Diff<Now>.Days), typeof(int))]
        [PXUIField(DisplayName = "Number of Days Unassigned")]
        public virtual int? TimeWithoutAction { get; set; }
        public abstract class timeWithoutAction : PX.Data.BQL.BqlInt.Field<timeWithoutAction> { }
        #endregion

        #region DefaultAssignee
        [PXInt]
        [PXUIField(DisplayName = "Default Assignee")]
        public virtual int? DefaultAssignee { get; set; }
        public abstract class defaultAssignee : PX.Data.BQL.BqlInt.Field<defaultAssignee> { }
        #endregion

        #region AssignTo
        [PXInt]
        [PXUIField(DisplayName = "Assign To")]
        public virtual int? AssignTo { get; set; }
        public abstract class assignTo : PX.Data.BQL.BqlInt.Field<assignTo> { }
        #endregion

        #region NbrOfAssignedOrders
        [PXInt]
        [PXUIField(DisplayName = "Number of Assigned Work Orders")]
        public virtual int? NbrOfAssignedOrders { get; set; }
        public abstract class nbrOfAssignedOrders :
            PX.Data.BQL.BqlInt.Field<nbrOfAssignedOrders>
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

    [PXHidden]
    public class RSSVWorkOrderToAssign : RSSVWorkOrder
    {
        #region Status
        public new abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region Priority
        public new abstract class priority : PX.Data.BQL.BqlString.Field<priority> { }
        #endregion

        #region ServiceID
        public new abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region DateCreated
        public new abstract class dateCreated : PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
        #endregion

        #region Assignee
        public new abstract class assignee : PX.Data.BQL.BqlGuid.Field<assignee> { }
        #endregion

        #region TimeWithoutAction
        [PXInt]
        [PXDBCalced(
            typeof(RSSVWorkOrderToAssign.dateCreated.Diff<Now>.Days),
            typeof(int))]
        [PXUIField(DisplayName = "Number of Days Unassigned")]
        public virtual int? TimeWithoutAction { get; set; }
        public abstract class timeWithoutAction :
            PX.Data.BQL.BqlInt.Field<timeWithoutAction>
        { }
        #endregion
    }
}