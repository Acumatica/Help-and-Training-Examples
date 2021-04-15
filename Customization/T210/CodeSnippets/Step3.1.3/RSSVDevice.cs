using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;

namespace PhoneRepairShop
{
    [PXCacheName("Serviced Device")]
    [PXPrimaryGraph(typeof(RSSVDeviceMaint))]
    public class RSSVDevice : IBqlTable
    {
        ...
    }
}
