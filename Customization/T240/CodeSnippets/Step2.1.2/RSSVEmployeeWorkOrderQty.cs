using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXHidden]
    public class RSSVEmployeeWorkOrderQty : IBqlTable
    {
        #region Userid
        [PXDBInt(IsKey = true)]
        public virtual int? Userid { get; set; }
        public abstract class userid : PX.Data.BQL.BqlInt.Field<userid> { }
        #endregion

        #region NbrOfAssignedOrders
        [PXDBInt()]
        public virtual int? NbrOfAssignedOrders { get; set; }
        public abstract class nbrOfAssignedOrders : PX.Data.BQL.BqlInt.Field<nbrOfAssignedOrders> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion
    }
}
