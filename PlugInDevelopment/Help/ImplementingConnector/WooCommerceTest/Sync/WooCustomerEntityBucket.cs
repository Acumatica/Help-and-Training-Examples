using PX.Commerce.BigCommerce;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    public class WooCustomerEntityBucket : EntityBucketBase, IEntityBucket
    {
        public IMappedEntity Primary => Customer;
        public IMappedEntity[] Entities => new IMappedEntity[] { Customer };

        public MappedCustomer Customer;
    }
}
