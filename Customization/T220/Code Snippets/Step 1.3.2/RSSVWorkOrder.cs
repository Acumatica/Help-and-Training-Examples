using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
		...
		#region CustomerID
        [PXDefault]
        [CustomerActive(DisplayName = "Customer ID", DescriptionField =typeof(Customer.acctName))]
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion
		
		#region DeviceID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Device",
        Visibility = PXUIVisibility.SelectorVisible)]
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
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
        [PXUIEnabled(typeof(Where<RSSVWorkOrder.hold.IsEqual<True>>))]
        [PXSelector(typeof(Search<RSSVRepairService.serviceID>),
            typeof(RSSVRepairService.serviceCD),
            typeof(RSSVRepairService.description),
            SubstituteKey = typeof(RSSVRepairService.serviceCD),
            DescriptionField = typeof(RSSVRepairService.description))]
        public virtual int? ServiceID { get; set; }
        public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
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

		...

    }
}