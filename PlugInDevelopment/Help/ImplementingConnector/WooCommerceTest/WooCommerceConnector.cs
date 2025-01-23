using Newtonsoft.Json;
using System;
using PX.Commerce.BigCommerce.API.REST;
using PX.Commerce.Core.REST;
using PX.Commerce.Core;
using PX.Data.BQL;
using PX.Async;
using System.Collections.Generic;
using PX.Common;
using System.Linq;
using PX.Data;
using System.Threading.Tasks;
using System.Threading;
using RestSharp;

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
            ISyncDetail detail = null)
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
            ConnectorOperation operation, int?[] syncIDs = null,
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
            return GetRestClient(binding.StoreBaseUrl, 
                binding.StoreXAuthClient, binding.StoreXAuthToken);
        }

        public static RestClient GetRestClient(String url, String clientID, 
            String token)
        {
            RestOptions options = new RestOptions
            {
                BaseUri = url,
                XAuthClient = clientID,
                XAuthTocken = token
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
