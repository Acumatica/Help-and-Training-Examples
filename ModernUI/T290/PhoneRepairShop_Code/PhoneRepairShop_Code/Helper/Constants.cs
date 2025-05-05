using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneRepairShop
{
    public static class RepairComplexity
    {
        public const string Low = "L";
        public const string Medium = "M";
        public const string High = "H";
    }

    //Constants for the repair item types
    public static class RepairItemTypeConstants
    {
        public const string Battery = "BT";
        public const string Screen = "SR";
        public const string ScreenCover = "SC";
        public const string BackCover = "BC";
        public const string Motherboard = "MB";
    }

    //Constants for the priority of repair work orders
    public static class WorkOrderPriorityConstants
    {
        public const string High = "H";
        public const string Medium = "M";
        public const string Low = "L";
    }

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

    //Constants for the repair work order types
    public static class WorkOrderTypeConstants
    {
        public const string Simple = "SP";
        public const string Standard = "ST";
        public const string Awaiting = "AW";
    }

}
