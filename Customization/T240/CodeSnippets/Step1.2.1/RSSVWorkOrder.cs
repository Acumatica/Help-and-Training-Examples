using System;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.TM;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;

namespace PhoneRepairShop
{
    [PXHidden]
    public class RSSVWorkOrderToAssign : RSSVWorkOrder
    {
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        #endregion

        #region Status
        public new abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region Priority
        public new abstract class priority : PX.Data.BQL.BqlString.Field<priority>
        { }
        #endregion

        #region ServiceID
        public new abstract class serviceID : PX.Data.BQL.BqlInt.Field<serviceID>
        { }
        #endregion

        #region DateCreated
        public new abstract class dateCreated :
         PX.Data.BQL.BqlDateTime.Field<dateCreated>
        { }
        #endregion

        #region Assignee
        public new abstract class assignee : PX.Data.BQL.BqlGuid.Field<assignee>
        { }
        #endregion

        #region TimeWithoutAction
        [PXInt]
        [PXDBCalced(
            typeof(RSSVWorkOrderToAssign.dateCreated.Diff<Now>.Days),
            typeof(int))]
        [PXUIField(DisplayName = "Number of Days Unassigned")]
        public virtual int? TimeWithoutAction { get; set; }
        public abstract class timeWithoutAction :
            PX.Data.BQL.BqlInt.Field<timeWithoutAction>
        { }
        #endregion
    }
}