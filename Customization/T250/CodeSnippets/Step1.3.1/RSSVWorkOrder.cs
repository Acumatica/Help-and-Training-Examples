namespace PhoneRepairShop
{
    ...

    [PXCacheName("Repair Work Order to Pay")]
    public class RSSVWorkOrderToPay : RSSVWorkOrder
    {
        ...

        #region OrderType
        [PXString(IsKey = true)]
        [PXUIField(DisplayName = "Order Type")]
        [PXUnboundDefault(OrderTypeConstants.WorkOrder)]
        [PXStringList(
          new string[]
          {
              OrderTypeConstants.SalesOrder,
              OrderTypeConstants.WorkOrder
          },
          new string[]
          {
              Messages.SalesOrder,
              Messages.WorkOrder
          })]
        public virtual String OrderType { get; set; }
        public abstract class orderType :
            PX.Data.BQL.BqlDecimal.Field<orderType> { }
        #endregion
    }
}