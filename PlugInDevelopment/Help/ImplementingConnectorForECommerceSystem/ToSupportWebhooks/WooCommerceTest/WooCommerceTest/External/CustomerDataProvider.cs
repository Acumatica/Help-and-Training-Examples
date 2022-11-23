using System.Collections.Generic;

namespace WooCommerceTest
{
    public class CustomerDataProvider : RestDataProvider
    {
        protected override string GetListUrl { get; } = "/customers";

        protected override string GetSingleUrl { get; } = "/customers/{id}";

        public CustomerDataProvider(IWooRestClient restClient) : base()
        {
            _restClient = restClient;
        }

        public IEnumerable<CustomerData> GetAll(IFilter filter = null)
        {
            var localFilter = filter ?? new Filter();
            localFilter.OrderBy = "registered_date";

            return base.GetAll<CustomerData, List<CustomerData>>(filter);
        }

        public CustomerData GetCustomerById(int id)
        {
            var segments = CustomerDataProvider.MakeUrlSegments(id.ToString());
            return base.GetByID<CustomerData>(segments);
        }
    }
}
