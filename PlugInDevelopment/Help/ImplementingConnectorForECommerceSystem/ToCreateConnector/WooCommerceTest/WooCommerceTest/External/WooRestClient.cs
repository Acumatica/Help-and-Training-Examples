using PX.Commerce.Core;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Net;
using BigCom = PX.Commerce.BigCommerce.API.REST;

namespace WooCommerceTest
{
    public class WooRestClient : WooRestClientBase, IWooRestClient
    {
        public WooRestClient(IDeserializer deserializer, ISerializer serializer, BigCom.IRestOptions options, Serilog.ILogger logger)
            : base(deserializer, serializer, options, logger)
        {
        }

        public T Get<T>(string url)
            where T : class, new()
        {
            RestRequest request = MakeRequest(url);

            request.Method = Method.GET;
            var response = Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            {
                T result = response.Data;

                if (result != null && result is BCAPIEntity) (result as BCAPIEntity).JSON = response.Content;

                return result;
            }

            throw new Exception(response.Content);
        }

        public T Post<T>(IRestRequest request, T entity)
            where T : class, IWooEntity, new()
        {
            request.Method = Method.POST;
            request.AddJsonBody(entity);
            IRestResponse<T> response = Execute<T>(request);
            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                T result = response.Data;

                if (result != null && result is BCAPIEntity) (result as BCAPIEntity).JSON = response.Content;

                return result;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }

        public TE Post<T, TE>(IRestRequest request, List<T> entities)
            where T : class, IWooEntity, new()
            where TE : IEnumerable<T>, new()
        {
            request.Method = Method.POST;
            request.AddJsonBody(entities);
            IRestResponse<TE> response = Execute<TE>(request);
            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                TE result = response.Data;

                if (result != null && result is IEnumerable<BCAPIEntity>)
                    (result as List<BCAPIEntity>).ForEach(e => e.JSON = response.Content);

                return result;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }

        public T Put<T>(IRestRequest request, T entity)
            where T : class, new()
        {
            request.Method = Method.PUT;
            request.AddJsonBody(entity);

            var response = Execute<T>(request);
            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
            {
                T result = response.Data;

                if (result != null && result is BCAPIEntity) (result as BCAPIEntity).JSON = response.Content;

                return result;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }


        public T Get<T>(IRestRequest request)
            where T : class, new()
        {
            request.Method = Method.GET;
            var response = Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            {
                T result = response.Data;

                if (result != null && result is BCAPIEntity) (result as BCAPIEntity).JSON = response.Content;

                return result;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }
        public TE GetList<T, TE>(IRestRequest request)
            where T : class, IWooEntity, new()
            where TE : IEnumerable<T>, new()
        {
            request.Method = Method.GET;
            var response = Execute<TE>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
            {
                TE result = response.Data;

                return result;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }

        public bool Delete(IRestRequest request)
        {
            request.Method = Method.DELETE;
            var response = Execute(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            {
                return true;
            }

            LogError(BaseUrl, request, response);
            throw new BigCom.RestException(response);
        }
    }
}