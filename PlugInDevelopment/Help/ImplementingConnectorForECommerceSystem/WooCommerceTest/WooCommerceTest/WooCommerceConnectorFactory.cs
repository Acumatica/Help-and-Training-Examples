using PX.Commerce.Core;
using System.Collections.Generic;
using System.Linq;

namespace WooCommerceTest
{
    public class WooCommerceConnectorFactory : BaseConnectorFactory<WooCommerceConnector>, IConnectorFactory
    {
        public override string Description => WooCommerceConnector.NAME;
        public override bool Enabled => true;

        public override string Type => WooCommerceConnector.TYPE;

        public WooCommerceConnectorFactory(IEnumerable<IProcessorsFactory> processors)
            : base(processors)
        {
        }

        public IConnectorDescriptor GetDescriptor()
        {
            return new WooCommercesConnectorDescriptor(_processors.Values.ToList());
        }
    }
}
