using Newtonsoft.Json;
using System;
using PX.Commerce.BigCommerce.API.REST;
using PX.Commerce.Core.REST;
using PX.Commerce.Core;
using CommonServiceLocator;
using PX.Data.BQL;
using System.Collections.Generic;
using PX.Common;
using System.Linq;
using PX.Data;

namespace WooCommerceTest
{
    public class WooCommerceConnector: BCConnectorBase<WooCommerceConnector>, IConnector
    {
        public const string TYPE = "WOO";
        public const string NAME = "WooCommerce";

        public class WCConnectorType : BqlString.Constant<WCConnectorType>
        {
            public WCConnectorType() : base(TYPE) { }
        }

        public override string ConnectorType { get => TYPE; }
        public override string ConnectorName { get => NAME; }

        public virtual IEnumerable<TInfo> GetExternalInfo<TInfo>(string infoType, int? bindingID)
            where TInfo : class
        {
            if (string.IsNullOrEmpty(infoType) || bindingID == null) return null;
            BCBindingWooCommerce binding = BCBindingWooCommerce.PK.Find(this, bindingID);
            if (binding == null) return null;

            try
            {
                List<TInfo> result = new List<TInfo>();
                return result;
            }
            catch (Exception ex)
            {
                LogError(new BCLogTypeScope(typeof(WooCommerceConnector)), ex);
            }

            return null;
        }

        public void NavigateExtern(ISyncStatus status)
        {
            if (status?.ExternID == null) return;

            EntityInfo info = GetEntities().FirstOrDefault(e => e.EntityType == status.EntityType);
            BCBindingWooCommerce bCBindingBigCommerce = BCBindingWooCommerce.PK.Find(this, status.BindingID);

            if (string.IsNullOrEmpty(bCBindingBigCommerce?.StoreAdminUrl) || string.IsNullOrEmpty(info.URL)) return;

            string[] parts = status.ExternID.Split(new char[] { ';' });
            string url = string.Format(info.URL, parts.Length > 2 ? parts.Take(2).ToArray() : parts);
            string redirectUrl = bCBindingBigCommerce.StoreAdminUrl.TrimEnd('/') + "/" + url;

            throw new PXRedirectToUrlException(redirectUrl, PXBaseRedirectException.WindowMode.New, string.Empty);
        }

        public virtual SyncInfo[] Process(ConnectorOperation operation, int?[] syncIDs = null)
        {
            LogInfo(operation.LogScope(), BCMessages.LogConnectorStarted, NAME);

            EntityInfo info = GetEntities().FirstOrDefault(e => e.EntityType == operation.EntityType);
            using (IProcessor graph = (IProcessor)CreateInstance(info.ProcessorType))
            {
                graph.Initialise(this, operation);
                return graph.Process(syncIDs);
            }
        }

        public DateTime GetSyncTime(ConnectorOperation operation)
        {
            BCBindingWooCommerce binding = BCBindingWooCommerce.PK.Find(this, operation.Binding);
            //Acumatica Time
            PXDatabase.SelectDate(out DateTime dtLocal, out DateTime dtUtc);
            dtLocal = PX.Common.PXTimeZoneInfo.ConvertTimeFromUtc(dtUtc, PX.Common.LocaleInfo.GetTimeZone());


            return dtLocal;
        }

        public override void StartWebHook(string baseUrl, BCWebHook hook)
        {
            throw new NotImplementedException();
        }

        public virtual void ProcessHook(IEnumerable<BCExternQueueMessage> messages)
        {
            throw new NotImplementedException();
        }

        public override void StopWebHook(string baseUrl, BCWebHook hook)
        {
            throw new NotImplementedException();
        }

        public static WooRestClient GetRestClient(BCBindingWooCommerce binding)
        {
            return GetRestClient(binding.StoreBaseUrl, binding.StoreXAuthClient, binding.StoreXAuthToken);
        }

        public static WooRestClient GetRestClient(String url, String clientID, String token)
        {
            RestOptions options = new RestOptions
            {
                BaseUri = url,
                XAuthClient = clientID,
                XAuthTocken = token
            };
            JsonSerializer serializer = new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                ContractResolver = new GetOnlyContractResolver()
            };
            RestJsonSerializer restSerializer = new RestJsonSerializer(serializer);
            WooRestClient client = new WooRestClient(restSerializer, restSerializer, options,
                ServiceLocator.Current.GetInstance<Serilog.ILogger>());

            return client;
        }
    }
}
