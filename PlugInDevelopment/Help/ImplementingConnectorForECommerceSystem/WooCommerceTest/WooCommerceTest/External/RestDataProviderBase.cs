using PX.Commerce.Core;
using PX.Common;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using BigCom = PX.Commerce.BigCommerce.API.REST;

namespace WooCommerceTest
{
    public abstract class RestDataProviderBase
    {
        internal const string COMMERCE_RETRY_COUNT = "CommerceRetryCount";
        protected const int BATCH_SIZE = 10;
        protected const string ID_STRING = "id";
        protected const string PARENT_ID_STRING = "parent_id";
        protected const string OTHER_PARAM = "other_param";
        protected readonly int commerceRetryCount = WebConfig.GetInt(COMMERCE_RETRY_COUNT, 3);
        protected IWooRestClient _restClient;

        protected abstract string GetListUrl { get; }
        protected abstract string GetSingleUrl { get; }

        public RestDataProviderBase()
        {
        }

        public virtual T Create<T>(T entity, BigCom.UrlSegments urlSegments = null)
            where T : class, IWooEntity, new()
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .ForContext("Object", entity)
                .Verbose("{CommerceCaption}: WooCommerce REST API - Creating new {EntityType} entity with parameters {UrlSegments}", BCCaptions.CommerceLogCaption, typeof(T).ToString(), urlSegments?.ToString() ?? "none");

            int retryCount = 0;
            while (true)
            {
                try
                {
                    var request = _restClient.MakeRequest(GetListUrl, urlSegments?.GetUrlSegments());
                    request.Method = RestSharp.Method.POST;


                    T result = _restClient.Post<T>(request, entity);

                    return result;
                }
                catch (BigCom.RestException ex)
                {
                    if (ex?.ResponceStatusCode == default(HttpStatusCode).ToString() && retryCount < commerceRetryCount)
                    {
                        _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                            .Error("{CommerceCaption}: Operation failed, RetryCount {RetryCount}, Exception {ExceptionMessage}",
                            BCCaptions.CommerceLogCaption, retryCount, ex?.ToString());

                        retryCount++;
                        Thread.Sleep(1000 * retryCount);
                    }
                    else throw;
                }
            }
        }

        public virtual TE Create<T, TE>(List<T> entities, BigCom.UrlSegments urlSegments = null)
            where T : class, IWooEntity, new()
            where TE : IList<T>, new()
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .ForContext("Object", entities)
                .Verbose("{CommerceCaption}: WooCommerce REST API - Creating new {EntityType} entity with parameters {UrlSegments}", BCCaptions.CommerceLogCaption, typeof(T).ToString(), urlSegments?.ToString() ?? "none");

            int retryCount = 0;
            while (true)
            {
                try
                {
                    var request = _restClient.MakeRequest(GetListUrl, urlSegments?.GetUrlSegments());

                    TE result = _restClient.Post<T, TE>(request, entities);

                    return result;
                }
                catch (BigCom.RestException ex)
                {
                    if (ex?.ResponceStatusCode == default(HttpStatusCode).ToString() && retryCount < commerceRetryCount)
                    {
                        _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                            .Error("{CommerceCaption}: Operation failed, RetryCount {RetryCount}, Exception {ExceptionMessage}",
                            BCCaptions.CommerceLogCaption, retryCount, ex?.ToString());

                        retryCount++;
                        Thread.Sleep(1000 * retryCount);
                    }
                    else throw;
                }
            }
        }

        public virtual T Update<T>(T entity, BigCom.UrlSegments urlSegments)
            where T : class, new()
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .ForContext("Object", entity)
                .Verbose("{CommerceCaption}: WooCommerce REST API - Updating {EntityType} entity with parameters {UrlSegments}", BCCaptions.CommerceLogCaption, typeof(T).ToString(), urlSegments?.ToString() ?? "none");

            int retryCount = 0;
            while (true)
            {
                try
                {
                    var request = _restClient.MakeRequest(GetSingleUrl, urlSegments?.GetUrlSegments());

                    T result = _restClient.Put<T>(request, entity);

                    return result;
                }
                catch (BigCom.RestException ex)
                {
                    if (ex?.ResponceStatusCode == default(HttpStatusCode).ToString() && retryCount < commerceRetryCount)
                    {
                        _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                            .Error("{CommerceCaption}: Operation failed, RetryCount {RetryCount}, Exception {ExceptionMessage}",
                            BCCaptions.CommerceLogCaption, retryCount, ex?.ToString());

                        retryCount++;
                        Thread.Sleep(1000 * retryCount);
                    }
                    else throw;
                }
            }
        }

        public virtual bool Delete(BigCom.UrlSegments urlSegments)
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .Verbose("{CommerceCaption}: WooCommerce REST API - Deleting {EntityType} entry with parameters {UrlSegments}", BCCaptions.CommerceLogCaption, GetType().ToString(), urlSegments?.ToString() ?? "none");

            int retryCount = 0;
            while (true)
            {
                try
                {
                    var request = _restClient.MakeRequest(GetSingleUrl, urlSegments.GetUrlSegments());

                    var result = _restClient.Delete(request);

                    return result;
                }
                catch (BigCom.RestException ex)
                {
                    if (ex?.ResponceStatusCode == default(HttpStatusCode).ToString() && retryCount < commerceRetryCount)
                    {
                        _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                            .Error("{CommerceCaption}: Operation failed, RetryCount {RetryCount}, Exception {ExceptionMessage}",
                            BCCaptions.CommerceLogCaption, retryCount, ex?.ToString());

                        retryCount++;
                        Thread.Sleep(1000 * retryCount);
                    }
                    else throw;
                }
            }
        }

        protected static BigCom.UrlSegments MakeUrlSegments(string id)
        {
            var segments = new BigCom.UrlSegments();
            segments.Add(ID_STRING, id);
            return segments;
        }

        protected static BigCom.UrlSegments MakeParentUrlSegments(string parentId)
        {
            var segments = new BigCom.UrlSegments();
            segments.Add(PARENT_ID_STRING, parentId);

            return segments;
        }


        protected static BigCom.UrlSegments MakeUrlSegments(string id, string parentId)
        {
            var segments = new BigCom.UrlSegments();
            segments.Add(PARENT_ID_STRING, parentId);
            segments.Add(ID_STRING, id);
            return segments;
        }
        protected static BigCom.UrlSegments MakeUrlSegments(string id, string parentId, string param)
        {
            var segments = new BigCom.UrlSegments();
            segments.Add(PARENT_ID_STRING, parentId);
            segments.Add(ID_STRING, id);
            segments.Add(OTHER_PARAM, param);
            return segments;
        }
    }


}
