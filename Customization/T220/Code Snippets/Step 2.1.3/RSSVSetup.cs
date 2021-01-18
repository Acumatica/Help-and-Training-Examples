using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;
using PX.Objects.CS;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order Preferences")]
    [PXPrimaryGraph(typeof(RSSVSetupMaint))]
    public class RSSVSetup : IBqlTable
    {
        ...
        #region NumberingID
        [PXDBString(10, IsUnicode = true)]
        [PXDefault("WORKORDER")]
        [PXUIField(DisplayName = "Numbering Sequence")]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        public virtual string NumberingId { get; set; }
        //not in tutorial
        public abstract class numberingID : PX.Data.BQL.BqlString.Field<numberingID> { }
        #endregion
		...
    }
}
