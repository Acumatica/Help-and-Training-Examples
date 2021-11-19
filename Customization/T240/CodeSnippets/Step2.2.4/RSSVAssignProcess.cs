using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PhoneRepairShop.Workflows;
using PX.TM;
using System.Collections.Generic;
using PX.Data.BQL;
using System.Linq;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        ...

        protected virtual void _(Events.FieldSelecting<RSSVWorkOrder,
                         RSSVWorkOrder.nbrOfAssignedOrders> e)
        {
            if (e.Row == null) return;
            RSSVEmployeeWorkOrderQty employeeNbrOfOrders =
                SelectFrom<RSSVEmployeeWorkOrderQty>.
                Where<RSSVEmployeeWorkOrderQty.userid.IsEqual<@P.AsInt>>.
                    View.Select(this, e.Row.AssignTo);
            if (employeeNbrOfOrders != null)
            {
                e.ReturnValue =  employeeNbrOfOrders.NbrOfAssignedOrders.
                    GetValueOrDefault();
            }
            else
            {
                e.ReturnValue = 0;
            }
        }

        ...
    }
}