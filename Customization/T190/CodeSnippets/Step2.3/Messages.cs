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
        //DAC names
        public const string RSSVDevice = "Serviced Device";
        public const string RSSVRepairService = "Repair Service";
        public const string RSSVRepairPrice = "Repair Price";
        public const string RSSVRepairItem = "Repair Item";
        public const string RSSVStockItemDevice = "Device Compatible with Stock Item";
        public const string RSSVLabor = "Repair Labor";
        public const string RSSVWarranty = "Warranty";
        public const string RSSVWorkOrder = "Repair Work Order";
        public const string RSSVWorkOrderItem = "Repair Item Included in Repair Work Order";
        public const string RSSVWorkOrderLabor = "Work Order Labor";
        public const string RSSVSetup = "Repair Work Order Preferences";
		
        //Complexity of repair
        public const string High = "High";
        public const string Medium = "Medium";
        public const string Low = "Low";
        ////////// The added code
        //Repair item types
        public const string Battery = "Battery";
        public const string Screen = "Screen";
        public const string ScreenCover = "Screen Cover";
        public const string BackCover = "Back Cover";
        public const string Motherboard = "Motherboard";
        ////////// The end of added code
        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";
        public const string ItemIsStock = "This item is a stock item.";
        public const string DefaultWarrantyCanNotBeDeleted =
        "The default warranty cannot be deleted.";

        //Work order statuses
        public const string OnHold = "On Hold";
        public const string PendingPayment = "Pending Payment";
        public const string ReadyForAssignment = "Ready for Assignment";
        public const string Assigned = "Assigned";
        public const string Completed = "Completed";
        public const string Paid = "Paid";

        public const string NoRequiredItem = "The work order does not contain a required repair item of the {0} type.";
        public const string PriorityTooLow =
            @"The priority must be at least Medium for the repair service that requires preliminary check.";
    }
}
