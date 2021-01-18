using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
	...
    //Constants for the statuses of repair work orders
    public static class WorkOrderStatusConstants
    {
        public const string OnHold = "OH";
        public const string PendingPayment = "PP";
        public const string ReadyForAssignment = "RA";
        public const string Assigned = "AS";
        public const string Completed = "CM";
        public const string Paid = "PD";
    }
    //Constants for the priority of repair work orders
    public static class WorkOrderPriorityConstants
    {
        public const string High = "H";
        public const string Medium = "M";
        public const string Low = "L";
    }
}