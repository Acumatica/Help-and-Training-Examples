namespace PhoneRepairShop
{
    ...

    [PXCacheName("Repair Work Order to Pay")]
    public class RSSVWorkOrderToPay : RSSVWorkOrder
    {
        ...

        public new abstract class serviceID :
            PX.Data.BQL.BqlInt.Field<serviceID> { }

        public new abstract class customerID :
            PX.Data.BQL.BqlInt.Field<customerID> { }
    }
}