using System;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.TM;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;

namespace PhoneRepairShop
{
    [Serializable]
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        #endregion

        ...
    }
}