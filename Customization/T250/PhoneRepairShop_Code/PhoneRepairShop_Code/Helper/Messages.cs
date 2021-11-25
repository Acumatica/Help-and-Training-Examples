using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Common;

namespace PhoneRepairShop
{
    [PXLocalizable()]
    public static class Messages
    {
        //Complexity of repair
        public const string High = "High";
        public const string Medium = "Medium";
        public const string Low = "Low";

        //Repair item types
        public const string Battery = "Battery";
        public const string Screen = "Screen";
        public const string ScreenCover = "Screen Cover";
        public const string BackCover = "Back Cover";
        public const string Motherboard = "Motherboard";

        //Work order statuses
        public const string OnHold = "On Hold";
        public const string PendingPayment = "Pending Payment";
        public const string ReadyForAssignment = "Ready for Assignment";
        public const string Assigned = "Assigned";
        public const string Completed = "Completed";
        public const string Paid = "Paid";

        // Order types
        public const string SalesOrder = "SO";
        public const string WorkOrder = "WO";

        //Messages
        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";
        public const string ItemIsStock = "This item is a stock item.";
        public const string DefaultWarrantyCanNotBeDeleted =
            "The default warranty cannot be deleted.";


        public const string QuantityCannotBeNegative =
            "The value in the Quantity column cannot be negative.";
        public const string QuantityToSmall = @"The value in the Quantity column 
            has been corrected to the minimum possible value.";


        public const string PriorityTooLow =
            @"The priority must be at least Medium for the repair service that requires preliminary check.";
    }
}
