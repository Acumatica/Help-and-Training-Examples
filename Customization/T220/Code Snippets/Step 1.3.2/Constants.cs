using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
	...
   
    //The fluent BQL constants for the work order statuses used in PXUIVisible
    public class workOrderStatusOnHold :
    PX.Data.BQL.BqlString.Constant<workOrderStatusOnHold>
    {
        public workOrderStatusOnHold()
        : base(WorkOrderStatusConstants.OnHold)
        {
        }
    }
    public class workOrderStatusReadyForAssignment :
    PX.Data.BQL.BqlString.Constant<workOrderStatusReadyForAssignment>
    {
        public workOrderStatusReadyForAssignment()
        : base(WorkOrderStatusConstants.ReadyForAssignment)
        {
        }
    }
    public class workOrderStatusPendingPayment :
    PX.Data.BQL.BqlString.Constant<workOrderStatusPendingPayment>
    {
        public workOrderStatusPendingPayment()
        : base(WorkOrderStatusConstants.PendingPayment)
        {
        }
    }
}
