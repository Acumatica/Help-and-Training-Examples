using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;
using PX.Objects.CS;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Order Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
        [AutoNumber(typeof(RSSVSetup.numberingID), typeof(RSSVWorkOrder.dateCreated))]
        [PXSelector(typeof(Search<RSSVWorkOrder.orderNbr>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion
        
        ...
    }
}