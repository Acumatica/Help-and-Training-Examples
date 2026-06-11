using PX.Common;
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

    //////////////////////// The added code

    // Constants for the Record Insight Panel's status property
    [PXLocalizable]
    public static class PanelStatus
    {
        public const string NeedsAttention = "Needs Attention";
        public const string InProgress = "In Progress";
        public const string Completed = "Completed";
        public const string Invoiced = "Invoiced";
    }

    // Constants for the Record Insight Panel's warning message property
    [PXLocalizable]
    public static class PanelWarningMessages
    {
        public const string OnHoldWarning =
            "The repair work order is on hold and must be reviewed before further processing.";

        public const string NoAssigneeWarning =
            "No assignee has been selected for this repair work order.";
    }

    // Constants for the Record Insight Panel's details property
    [PXLocalizable]
    public static class PanelDetailsMessages
    {
        public const string OnHoldDetails =
            "Check the priority and required repair items before removing the hold.";

        public const string NoAssigneeDetails =
            "Assign an employee to this repair work order.";

        public const string InProgressDetails =
            "The repair work order is being processed and has not been completed yet.";

        public const string CompleteDetails =
            "The repair work order has been completed but has not yet been linked to an invoice.";

        public const string ReadyDetails =
            "The repair work order has been completed and has been linked to an invoice.";
    }

    // Constants for the Record Insight Panel's accent property
    public static class PanelAccent
    {
        public const string Danger = "danger";
        public const string Neutral = "neutral";
        public const string Warning = "warning";
        public const string Success = "success";
    }

    //////////////////////// The end of the added code


}
