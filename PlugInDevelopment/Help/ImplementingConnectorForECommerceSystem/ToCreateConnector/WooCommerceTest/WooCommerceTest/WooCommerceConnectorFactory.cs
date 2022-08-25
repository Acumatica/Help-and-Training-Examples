using PX.Commerce.Core;
using System;

namespace WooCommerceTest
{
    public class WooCommerceConnectorFactory : BaseConnectorFactory<WooCommerceConnector>, IConnectorFactory
    {
        public override string Description => WooCommerceConnector.NAME;
        public override bool Enabled => true;

        public override string Type => WooCommerceConnector.TYPE;

        public WooCommerceConnectorFactory(ProcessorFactory factory)
            : base(factory)
        {
        }

        public virtual Guid? GenerateExternID(BCExternNotification message)
        {
            throw new NotImplementedException();
        }
    }
}
