using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXHidden]
    public class RSSVEmployeeWorkOrderQty : PXBqlTable, IBqlTable
    {
        #region UserID
        [PXDBInt(IsKey = true)]
        public virtual int? UserID { get; set; }
        public abstract class userID : PX.Data.BQL.BqlInt.Field<userID> { }
        #endregion

        #region NbrOfAssignedOrders
        [PXDBInt()]
        public virtual int? NbrOfAssignedOrders { get; set; }
        public abstract class nbrOfAssignedOrders : PX.Data.BQL.BqlInt.Field<nbrOfAssignedOrders> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime :
            PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime>
        { }
        #endregion
    }
}