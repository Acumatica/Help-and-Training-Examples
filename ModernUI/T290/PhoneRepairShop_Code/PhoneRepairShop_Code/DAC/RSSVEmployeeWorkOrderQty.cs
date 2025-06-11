using System;
using PX.Data;

namespace PhoneRepairShop
{
    [PXHidden]
    [RSSVEmployeeWorkOrderQtyAccumulator]
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

    public class RSSVEmployeeWorkOrderQtyAccumulator :
    PXAccumulatorAttribute
    {
        //Specify the single-record mode of update in the constructor.
        public RSSVEmployeeWorkOrderQtyAccumulator()
        {
            _SingleRecord = true;
        }
        //Override the PrepareInsert method.
        protected override bool PrepareInsert(PXCache sender, object row,
            PXAccumulatorCollection columns)
        {
            if (!base.PrepareInsert(sender, row, columns)) return false;
            RSSVEmployeeWorkOrderQty newQty = (RSSVEmployeeWorkOrderQty)row;
            if (newQty.NbrOfAssignedOrders != null)
            {
                // Add the restriction for the value of 
                // RSSVEmployeeWorkOrderQty.NbrOfAssignedOrders.
                columns.AppendException(
                    Messages.ExceedingMaximumNumberOfAssignedWorkOrders,
                new PXAccumulatorRestriction<
                    RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders>(
                    PXComp.LE, 10));
            }
            // Update NbrOfAssignedOrders by using Summarize.
            columns.Update<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders>(
                newQty.NbrOfAssignedOrders,
                PXDataFieldAssign.AssignBehavior.Summarize);
            return true;
        }
    }
}