using RestSharp;
using System;
using System.Collections.Generic;

namespace WooCommerceTest
{
    public class CustomerDataProvider
    {
        private RestClient _restClient;

        protected string GetListUrl { get; } = "/customers";

        protected string GetSingleUrl { get; } = "/customers/{id}";

        public CustomerDataProvider(RestClient restClient) : base()
        {
            _restClient = restClient;
        }

        public IEnumerable<CustomerData> GetAll()
        {
            throw new NotImplementedException();
        }

        public CustomerData GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
