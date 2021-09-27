using PX.Commerce.Core;
using PX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BigCom = PX.Commerce.BigCommerce.API.REST;

namespace WooCommerceTest
{
    public abstract class RestDataProvider : RestDataProviderBase
    {
        public RestDataProvider() : base()
        {

        }

        public virtual TE Get<T, TE>(IFilter filter = null, BigCom.UrlSegments urlSegments = null)
            where T : class, IWooEntity, new()
            where TE : IEnumerable<T>, new()
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .Verbose("{CommerceCaption}: WooCommerce REST API - Getting {EntityType} entry with parameters {UrlSegments}",
                BCCaptions.CommerceLogCaption, GetType().ToString(), urlSegments?.ToString() ?? "none");

            var request = _restClient.MakeRequest(GetListUrl, urlSegments?.GetUrlSegments());
            filter?.AddFilter(request);

            var response = _restClient.GetList<T, TE>(request);
            return response;
        }

        public virtual IEnumerable<T> GetAll<T, TE>(IFilter filter = null, BigCom.UrlSegments urlSegments = null)
            where T : class, IWooEntity, new()
            where TE : IEnumerable<T>, new()
        {
            var localFilter = filter ?? new Filter();
            var needGet = true;

            localFilter.Page = localFilter.Page.HasValue ? localFilter.Page : 1;
            localFilter.Limit = localFilter.Limit.HasValue ? localFilter.Limit : 50;
            localFilter.Order = "desc";

            TE entity = default;
            while (needGet)
            {
                int retryCount = 0;
                while (retryCount <= commerceRetryCount)
                {
                    try
                    {
                        entity = Get<T, TE>(localFilter, urlSegments);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                            .Verbose("{CommerceCaption}: Failed at page {Page}, RetryCount {RetryCount}, Exception {ExceptionMessage}", BCCaptions.CommerceLogCaption, localFilter.Page, retryCount, ex?.Message);

                        if (retryCount == commerceRetryCount)
                            throw;

                        retryCount++;
                        Thread.Sleep(1000 * retryCount);
                    }
                }

                if (entity == null) yield break;
                foreach (T data in entity)
                {
                    yield return data;
                }


                if (entity.Count() < localFilter.Limit)
                    needGet = false;
                else if (entity.Count() == localFilter.Limit && localFilter.CreatedAfter != null
                    && localFilter.CreatedAfter > entity.ToList()[localFilter.Limit.Value - 1].DateCreatedUT)
                    needGet = false;


                localFilter.Page++;
            }
        }

        public virtual T GetByID<T>(BigCom.UrlSegments urlSegments, IFilter filter = null)
            where T : class, new()
        {
            _restClient.Logger?.ForContext("Scope", new BCLogTypeScope(GetType()))
                .Verbose("{CommerceCaption}: WooCommerce REST API - Getting by ID {EntityType} entry with parameters {UrlSegments}", BCCaptions.CommerceLogCaption, typeof(T).ToString(), urlSegments?.ToString() ?? "none");

            var request = _restClient.MakeRequest(GetSingleUrl, urlSegments.GetUrlSegments());
            if (filter != null)
                filter.AddFilter(request);
            return _restClient.Get<T>(request);
        }

        public virtual bool Delete(IFilter filter = null)
        {
            var request = _restClient.MakeRequest(GetSingleUrl);
            filter?.AddFilter(request);

            var response = _restClient.Delete(request);
            return response;
        }
    }


}
