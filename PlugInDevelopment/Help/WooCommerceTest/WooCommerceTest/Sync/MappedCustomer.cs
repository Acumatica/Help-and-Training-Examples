using System;
using PX.Commerce.Core;
using PX.Commerce.Core.API;

namespace WooCommerceTest
{
    public abstract class WCMappedEntity<ExternType, LocalType> : MappedEntity<ExternType, LocalType>
        where ExternType : BCAPIEntity, IExternEntity
        where LocalType : CBAPIEntity, ILocalEntity
    {
        public WCMappedEntity(string entType)
            : base(WooCommerceConnector.TYPE, entType)
        { }
        public WCMappedEntity(BCSyncStatus status)
            : base(status)
        {
        }
        public WCMappedEntity(string entType, LocalType entity, Guid? id, DateTime? timestamp)
            : base(WooCommerceConnector.TYPE, entType, entity, id, timestamp)
        {
        }
        public WCMappedEntity(string entType, ExternType entity, string id, DateTime? timestamp)
            : base(WooCommerceConnector.TYPE, entType, entity, id, timestamp)
        {
        }
        public WCMappedEntity(string entType, ExternType entity, string id, string hash)
            : base(WooCommerceConnector.TYPE, entType, entity, id, hash)
        {
        }
    }
    public class MappedCustomer : WCMappedEntity<CustomerData, Customer>
    {
        public const string TYPE = BCEntitiesAttribute.Customer;

        public MappedCustomer()
            : base(TYPE)
        { }
        public MappedCustomer(Customer entity, Guid? id, DateTime? timestamp)
            : base(TYPE, entity, id, timestamp) { }
        public MappedCustomer(CustomerData entity, string id, DateTime? timestamp)
            : base(TYPE, entity, id, timestamp) { }
    }
}
