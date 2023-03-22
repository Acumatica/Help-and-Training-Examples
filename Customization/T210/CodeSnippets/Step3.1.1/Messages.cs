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
        ////////// The added code
        public const string RSSVStockItemDevice = "Device Compatible with Stock Item";
        ////////// The end of added code

        //Messages
        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";
        
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
    }
}
