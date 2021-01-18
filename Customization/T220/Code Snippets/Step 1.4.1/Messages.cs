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

        //To validate the value of the Quantity field
        public const string QuantityCannotBeNegative ="The value in the Quantity column cannot be negative.";
        public const string QuantityToSmall = @"The value in the Quantity column has been corrected to the minimum possible value.";
    }
}
