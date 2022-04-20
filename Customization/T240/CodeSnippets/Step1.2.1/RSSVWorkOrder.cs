using System;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.TM;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    [PXPrimaryGraph(typeof(RSSVWorkOrderEntry))]
    public class RSSVWorkOrder : IBqlTable
    {
        ...

        #region TimeWithoutAction
        [PXInt]
        [PXDBCalced(
            typeof(RSSVWorkOrder.dateCreated.Diff<Now>.Days),
            typeof(int))]
        [PXUIField(DisplayName = "Number of Days Unassigned")]
        public virtual int? TimeWithoutAction { get; set; }
        public abstract class timeWithoutAction :
            PX.Data.BQL.BqlInt.Field<timeWithoutAction>
        { }
        #endregion
        
        ...
    }
}