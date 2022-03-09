using PX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Messages
        public const string StockItemIncorrectRepairItemType =
            "This stock item has a repair item type that differs from {0}.";
        public const string ItemIsStock = "This item is a stock item.";
        public const string DefaultWarrantyCanNotBeDeleted =
            "The default warranty cannot be deleted.";
    }
}
