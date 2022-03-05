using System;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.TM;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order Preferences")]
    [PXPrimaryGraph(typeof(RSSVSetupMaint))]
    public class RSSVSetup : IBqlTable
    {
        #region NumberingID
        [PXDefault("WORKORDER")]
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Numbering Sequence")]
        public virtual string NumberingID { get; set; }
        public abstract class numberingID : PX.Data.BQL.BqlString.Field<numberingID> { }
        #endregion

        ...
    }
}