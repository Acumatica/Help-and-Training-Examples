using PX.Commerce.BigCommerce.API.REST;
using PX.Commerce.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WooCommerceTest
{
    public class WebHookRestDataProvider : RestDataProvider
    {
        protected override string GetListUrl { get; } = "webhooks";
        protected override string GetSingleUrl { get; } = "webhooks/{id}";

        public WebHookRestDataProvider(IWooRestClient restClient) : base()
        {
            _restClient = restClient;
        }

        #region IParentDataRestClient
        public virtual WebHookData Create(WebHookData webhook)
        {
            var newwebhook = base.Create(webhook);
            return newwebhook;
        }

        public virtual WebHookData Update(WebHookData customer, string id)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(string id)
        {
            var segments = MakeUrlSegments(id);
            return Delete(segments);
        }

        public virtual bool Delete(string id, WebHookData order)
        {
            return Delete(id);
        }

        /*public virtual IEnumerable<WebHookData> GetAll(IFilter filter = null, CancellationToken cancellationToken = default)
        {
            return base.GetAll<WebHookData>(filter, cancellationToken: cancellationToken);
        }*/

        public virtual WebHookData GetByID(string webhookId)
        {
            var segments = MakeUrlSegments(webhookId);
            var customer = GetByID<WebHookData>(segments);
            return customer;
        }
        #endregion
    }
}
