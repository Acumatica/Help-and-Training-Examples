namespace PhoneRepairShop
{
    ...

    [PXHidden]
    public class RSSVWorkOrderToPayFilter : IBqlTable
    {
        ...

        #region GroupByStatus
        [PXBool]
        [PXUIField(DisplayName = "Show Total Amount to Pay")]
        public bool? GroupByStatus { get; set; }
        public abstract class groupByStatus :
            PX.Data.BQL.BqlBool.Field<groupByStatus> { }
        #endregion
    }
}