using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
        #region RepairItemLineCntr
        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? RepairItemLineCntr { get; set; }
        public abstract class repairItemLineCntr :
        PX.Data.BQL.BqlInt.Field<repairItemLineCntr>
        { }
        #endregion

        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Order Nbr.",
        Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<RSSVWorkOrder.orderNbr>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region Description
        [PXDBString(60, IsUnicode = true)]
        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
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
        public virtual int? DeviceID { get; set; }
        public abstract class deviceID : PX.Data.BQL.BqlInt.Field<deviceID> { }
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
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
        #endregion

        #region CustomerID
        [PXDefault]
        [CustomerActive(DisplayName = "Customer ID", DescriptionField =typeof(Customer.acctName))]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion

        #region Assignee
        [Owner(DisplayName = "Assignee")]
        public virtual int? Assignee { get; set; }
        public abstract class assignee : PX.Data.BQL.BqlInt.Field<assignee> { }
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
        public abstract class dateCompleted : PX.Data.BQL.BqlDateTime.Field<dateCompleted>
        { }
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

        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Hold")]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region OrderTotal
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Order Total", Enabled = false)]
        public virtual Decimal? OrderTotal { get; set; }
        public abstract class orderTotal : PX.Data.BQL.BqlDecimal.Field<orderTotal> { }
        #endregion

        #region InvoiceNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Invoice Nbr.", Enabled = false)]
        public virtual String InvoiceNbr { get; set; }
        public abstract class invoiceNbr : PX.Data.BQL.BqlString.Field<invoiceNbr> { }
        #endregion

	//system fields
	}
}