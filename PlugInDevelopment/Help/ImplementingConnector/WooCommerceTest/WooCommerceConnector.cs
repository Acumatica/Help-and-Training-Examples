using Newtonsoft.Json;
using PX.Async;
using PX.Commerce.BigCommerce;
using PX.Commerce.BigCommerce.API.REST;
using PX.Commerce.Core;
using PX.Commerce.Core.REST;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WooCommerceTest
{
    public class WooCommerceConnector : BCConnectorBase<WooCommerceConnector>
    {
        public const string TYPE = "WOO";
        public const string NAME = "WooCommerce";

        public class WCConnectorType : BqlString.Constant<WCConnectorType>
        {
            public WCConnectorType() : base(TYPE) { }
        }

        public override string ConnectorType { get => TYPE; }
        public override string ConnectorName { get => NAME; }

        public override void NavigateExtern(ISyncStatus status, 
            ISyncDetail detail)
        {
            if (status?.ExternID == null) return;

            EntityInfo info = GetEntities().FirstOrDefault(e => 
                e.EntityType == status.EntityType);
            BCBindingWooCommerce bCBindingBigCommerce = 
                BCBindingWooCommerce.PK.Find(this, status.BindingID);

            if (string.IsNullOrEmpty(bCBindingBigCommerce?.StoreAdminUrl) || 
                string.IsNullOrEmpty(info.URL)) return;

            string[] parts = status.ExternID.Split(new char[] { ';' });
            string url = string.Format(info.URL, parts.Length > 2 ? 
                parts.Take(2).ToArray() : parts);
            string redirectUrl = 
                bCBindingBigCommerce.StoreAdminUrl.TrimEnd('/') + "/" + url;

            throw new PXRedirectToUrlException(redirectUrl, 
                PXBaseRedirectException.WindowMode.New, string.Empty);
        }

        public override async Task<ConnectorOperationResult> Process(
            ConnectorOperation operation, int?[] syncIDs,
            CancellationToken cancellationToken = default)
        {
            EntityInfo info = GetEntities().FirstOrDefault(e => 
                e.EntityType == operation.EntityType);
            using (IProcessor graph = (IProcessor)CreateInstance(
                info.ProcessorType))
            {
                await graph.Initialise(this, operation);
                return await graph.Process(syncIDs, cancellationToken);
            }
        }

        public override async Task<DateTime> GetSyncTime(ConnectorOperation operation)
        {
            BCBindingWooCommerce binding = BCBindingWooCommerce.PK.Find(this, 
                operation.Binding);
            //Acumatica Time
            PXDatabase.SelectDate(out DateTime dtLocal, out DateTime dtUtc);
            dtLocal = PX.Common.PXTimeZoneInfo.ConvertTimeFromUtc(dtUtc, 
                PX.Common.LocaleInfo.GetTimeZone());

            return dtLocal;
        }

        public override async Task ProcessHook(
            IEnumerable<BCExternQueueMessage> messages, 
                CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public static RestClient GetRestClient(BCBindingWooCommerce binding)
        {
            Dictionary<string, string> authHeaders = new()
            {
                { BigCommerceConstants.Headers.AuthToken, binding.StoreXAuthToken },
                { BigCommerceConstants.Headers.AuthClient, binding.StoreXAuthClient }
            };
            return CreateClient(binding.StoreBaseUrl, authHeaders);
        }

        public static RestClient CreateClient(string baseUri, Dictionary<string, string> authHeaders)
        {
            RestOptions options = new RestOptions
            {
                BaseUri = baseUri,
                AuthHeaders = authHeaders
            };
            JsonSerializerSettings serializer = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                ContractResolver = new GetOnlyContractResolver()
            };

            RestClient client = new RestClient();

            return client;
        }

        public List<Tuple<string, string, string>> GetExternalFields(
            string type, int? binding, string entity)
        {
            List<Tuple<string, string, string>> fieldsList = 
                new List<Tuple<string, string, string>>();
            if (entity != BCEntitiesAttribute.Customer && entity != 
                BCEntitiesAttribute.Address) return fieldsList;

            return fieldsList;
        }

        public override Task StartWebHook(string baseUrl, BCWebHook hook, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task StopWebHook(string baseUrl, BCWebHook hook, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
