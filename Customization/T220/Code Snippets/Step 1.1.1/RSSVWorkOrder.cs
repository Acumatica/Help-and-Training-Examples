using System;
using PX.Data;

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
	...
    }
}