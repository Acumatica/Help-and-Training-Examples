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

        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";

        public const string CannotAddStockItemToRepairPrice =
            "Cannot add a stock item to a repair price.";
        public const string DefaultWarrantyCanNotBeDeleted = "This record cannot be deleted.";

        //Work order statuses
        public const string OnHold = "On Hold";
        public const string PendingPayment = "Pending Payment";
        public const string ReadyForAssignment = "Ready for Assignment";
        public const string Assigned = "Assigned";
        public const string Completed = "Completed";
        public const string Paid = "Paid";


        public const string QuantityCannotBeNegative = "The value in the Quantity column cannot be negative.";
        public const string QuantityToSmall = @"The value in the Quantity column has been 
             corrected to the minimum possible value.";

        public const string NoRequiredItem = "The work order does not contain a required repair item of the {0} type.";

        public const string WorkOrderAssigned =
            "The {0} work order has been successfully assigned.";
        
        public const string ExceedingMaximumNumberOfAssingedWorkOrders =
            @"Updating the number of assigned work orders for the employee 
            will lead to exceeding of the maximum number of assigned work orders, 
            which is 10.";

        public const string ReportRS601000Title = "Assigned Work Orders";

        // Order types
        public const string SalesOrder = "SO";
        public const string WorkOrder = "WO";
    }
}
