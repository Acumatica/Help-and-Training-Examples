using System;
using System.Collections.Generic;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    public class WooCommerceProcessorsFactory : IProcessorsFactory
    {
        public string ConnectorType => WooCommerceConnector.TYPE;

        public IEnumerable<KeyValuePair<Type, int>> GetProcessorTypes()
        {
            yield return new KeyValuePair<Type, int>(typeof(WooCustomerProcessor), 20);
        }
    }
}
