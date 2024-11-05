using PX.Commerce.BigCommerce;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    public class WooLocationEntityBucket : EntityBucketBase, IEntityBucket
    {
        public IMappedEntity Primary => Address;
        public IMappedEntity[] Entities =>
          new IMappedEntity[] { Address, Customer };

        public override IMappedEntity[] PostProcessors =>
          new IMappedEntity[] { Customer };

        public MappedLocation Address;
        public MappedCustomer Customer;
    }
}
