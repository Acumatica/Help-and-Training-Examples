using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVDeviceMaint : PXGraph<RSSVDeviceMaint, RSSVDevice>
    {
        public SelectFrom<RSSVDevice>.View ServDevices = null!;
    }
}
