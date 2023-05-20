using RestSharp;
using Serilog;
using System.Collections.Generic;

namespace WooCommerceTest
{
    public interface IWooRestClient
    {
        RestRequest MakeRequest(string url, Dictionary<string, string> urlSegments = null);

        T Post<T>(IRestRequest request, T entity) where T : class, IWooEntity, new();
        TE Post<T, TE>(IRestRequest request, List<T> entities) where T : class, IWooEntity, new() where TE : IEnumerable<T>, new();
        T Put<T>(IRestRequest request, T entity) where T : class, new();
        T Get<T>(IRestRequest request) where T : class, new();
        TE GetList<T, TE>(IRestRequest request) where T : class, IWooEntity, new() where TE : IEnumerable<T>, new();
        ILogger Logger { set; get; }
        bool Delete(IRestRequest request);
    }
}