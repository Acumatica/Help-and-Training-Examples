using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
    ...

    public class workOrderStatusPaid :
    PX.Data.BQL.BqlString.Constant<workOrderStatusPendingPayment>
    {
        public workOrderStatusPaid()
        : base(WorkOrderStatusConstants.Paid)
        {
        }
    }
}
