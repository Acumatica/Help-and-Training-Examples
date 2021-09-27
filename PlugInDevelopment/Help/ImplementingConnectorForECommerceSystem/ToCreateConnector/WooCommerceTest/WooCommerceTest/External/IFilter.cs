using RestSharp;
using System;

namespace WooCommerceTest
{
    public interface IFilter
    {
        void AddFilter(IRestRequest request);
        int? Limit { get; set; }
        int? Page { get; set; }

        int? Offset { get; set; }

        string Order { get; set; }

        string OrderBy { get; set; }

        DateTime? CreatedAfter { get; set; }
    }
}
