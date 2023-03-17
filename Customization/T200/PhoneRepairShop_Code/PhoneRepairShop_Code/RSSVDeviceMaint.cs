using PX.Data;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    public class RSSVDeviceMaint : PXGraph<RSSVDeviceMaint, RSSVDevice>
    {
        public SelectFrom<RSSVDevice>.View ServDevices;
    }
}