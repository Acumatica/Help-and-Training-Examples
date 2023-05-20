using System;
using PX.Commerce.Core;
using PX.Commerce.Core.API;

namespace WooCommerceTest
{
    public class MappedCustomer : MappedEntity<CustomerData, Customer>
    {
        public const string TYPE = BCEntitiesAttribute.Customer;

        public MappedCustomer()
            : base(WooCommerceConnector.TYPE, TYPE)
        { }
        public MappedCustomer(Customer entity, Guid? id, DateTime? timestamp)
            : base(WooCommerceConnector.TYPE, TYPE, entity, id, timestamp) { }
        public MappedCustomer(CustomerData entity, string id, DateTime? timestamp)
            : base(WooCommerceConnector.TYPE, TYPE, entity, id, id,timestamp) { }
    }
}
