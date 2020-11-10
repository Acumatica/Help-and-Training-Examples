using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    public class RSSVPaymentPlanInq : PXGraph<RSSVPaymentPlanInq>
    {

		...

        #region Data Access Classes

        [PXHidden]
        public class RSSVWorkOrderToPayFilter : IBqlTable
        {
            #region ServiceID
            [PXInt()]
            [PXUIField(DisplayName = "Service")]
            [PXSelector(
                typeof(Search<RSSVRepairService.serviceID>),
                typeof(RSSVRepairService.serviceCD),
                typeof(RSSVRepairService.description),
                DescriptionField = typeof(RSSVRepairService.description),
                SelectorMode = PXSelectorMode.DisplayModeText)]
            public virtual int? ServiceID { get; set; }
            public abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID> { }
            #endregion

            #region CustomerID
            [CustomerActive(DisplayName = "Customer ID", Required = true)]
            public virtual int? CustomerID { get; set; }
            public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
            #endregion
        }

        #endregion


    }
}