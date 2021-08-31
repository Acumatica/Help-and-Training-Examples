

namespace WooCommerceTest
{
    public class SystemStatusProvider 
    {
        private readonly IWooRestClient _restClient;

        public SystemStatusProvider(IWooRestClient restClient)
        {
            _restClient = restClient;
        }


        public SystemStatusData Get()
        {
            const string resourceUrl = "/system_status";

            var request = _restClient.MakeRequest(resourceUrl);
            var country = _restClient.Get<SystemStatusData>(request);
            return country;
        }
    }
}
