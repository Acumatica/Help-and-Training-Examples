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
        ...

        public const string ExceedingMaximumNumberOfAssingedWorkOrders =
            @"Updating the number of assigned work orders for the employee 
            will lead to exceeding of the maximum number of assigned work orders, 
            which is 10.";
    }
}
