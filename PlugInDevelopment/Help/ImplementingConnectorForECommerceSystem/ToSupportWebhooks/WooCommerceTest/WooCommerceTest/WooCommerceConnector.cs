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
using System.Threading.Tasks;
using System.Threading;
using PX.Commerce.BigCommerce;

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

        public void NavigateExtern(ISyncStatus status, ISyncDetail detail = null)
        {
            if (status?.ExternID == null) return;

            EntityInfo info = GetEntities().FirstOrDefault(e => e.EntityType == status.EntityType);
            BCBindingWooCom bCBindingBigCommerce = BCBindingWooCom.PK.Find(this, status.BindingID);

            if (string.IsNullOrEmpty(bCBindingBigCommerce?.StoreAdminUrl) || string.IsNullOrEmpty(info.URL)) return;

            string[] parts = status.ExternID.Split(new char[] { ';' });
            string url = string.Format(info.URL, parts.Length > 2 ? parts.Take(2).ToArray() : parts);
            string redirectUrl = bCBindingBigCommerce.StoreAdminUrl.TrimEnd('/') + "/" + url;

            throw new PXRedirectToUrlException(redirectUrl, PXBaseRedirectException.WindowMode.New, string.Empty);
        }

        public virtual async Task<ConnectorOperationResult> Process(ConnectorOperation operation, int?[] syncIDs = null,
            CancellationToken cancellationToken = default)
        {
            LogInfo(operation.LogScope(), BCMessages.LogConnectorStarted, NAME);

            EntityInfo info = GetEntities().FirstOrDefault(e => e.EntityType == operation.EntityType);
            using (IProcessor graph = (IProcessor)CreateInstance(info.ProcessorType))
            {
                graph.Initialise(this, operation);
                return await graph.Process(syncIDs, cancellationToken);
            }
        }

        public DateTime GetSyncTime(ConnectorOperation operation)
        {
            BCBindingWooCom binding = BCBindingWooCom.PK.Find(this, operation.Binding);
            //Acumatica Time
            PXDatabase.SelectDate(out DateTime dtLocal, out DateTime dtUtc);
            dtLocal = PX.Common.PXTimeZoneInfo.ConvertTimeFromUtc(dtUtc, PX.Common.LocaleInfo.GetTimeZone());


            return dtLocal;
        }

        public override void StartWebHook(String baseUrl, BCWebHook hook)
        {
            BCBinding store = BCBinding.PK.Find(this, hook.ConnectorType, hook.BindingID);
            BCBindingWooCom storeWooCommerce = BCBindingWooCom.PK.Find(this, hook.BindingID);

            WebHookRestDataProvider restClient = new WebHookRestDataProvider(GetRestClient(storeWooCommerce));

            //URL and HASH
            string url = new Uri(baseUrl, UriKind.RelativeOrAbsolute).ToString();
            if (url.EndsWith("/"))
                url = url.TrimEnd('/');
            url += hook.Destination;
            string hashcode = hook.ValidationHash ?? String.Concat(PX.Data.Update.PXCriptoHelper.CalculateSHA(Guid.NewGuid().ToString()).Select(b => b.ToString("X2")));

            //Create a new Hook
            WebHookData webHook = new WebHookData();
            webHook.Scope = hook.Scope;
            webHook.Destination = url;
            if (hook.IsActive == true) webHook.Active = "active";

            String companyName = PXAccess.GetCompanyName();
            webHook.Headers = new Dictionary<string, string>();
            webHook.Headers["type"] = TYPE;
            webHook.Headers["validation"] = hashcode;
            if (!String.IsNullOrEmpty(companyName)) webHook.Headers["company"] = companyName;

            webHook = restClient.Create(webHook);

            //Saving
            hook.IsActive = true;
            hook.HookRef = webHook.Id;
            hook.StoreHash = webHook.StoreHash;
            hook.ValidationHash = hashcode;

            Hooks.Update(hook);
            Actions.PressSave();
        }
        public override void StopWebHook(String baseUrl, BCWebHook hook)
        {
            hook.IsActive = false;
            hook.HookRef = null;

            Hooks.Update(hook);
            Actions.PressSave();
        }

        public virtual async Task ProcessHook(IEnumerable<BCExternQueueMessage> messages, CancellationToken cancellationToken = default)
        {
            Dictionary<RecordKey, RecordValue<String>> toProcess = new Dictionary<RecordKey, RecordValue<String>>();
            foreach (BCExternQueueMessage message in messages)
            {
                WebHookMessage jResult = JsonConvert.DeserializeObject<WebHookMessage>(message.Json);

                string scope = jResult.Scope;
                string producer = jResult.Producer;
                string data = jResult.Data;
                DateTime? created = jResult.DateCreatedUT.ToDate();
                String storehash = producer.Substring(producer.LastIndexOf("/") + 1);

                foreach (BCWebHook hook in PXSelect<BCWebHook,
                    Where<BCWebHook.connectorType, Equal<BCConnector.bcConnectorType>,
                        And<BCWebHook.storeHash, Equal<Required<BCWebHook.storeHash>>,
                        And<BCWebHook.scope, Equal<Required<BCWebHook.scope>>>>>>.Select(this, storehash, scope))
                {
                    if (hook.ValidationHash != message.Validation)
                    {
                        LogError(new BCLogTypeScope(typeof(BCConnector)), new PXException(BCMessages.WrongValidationHash, storehash ?? "", scope));
                        continue;
                    }

                    foreach (EntityInfo info in this.GetEntities().Where(e => e.ExternRealtime.Supported && e.ExternRealtime.WebHookType != null && e.ExternRealtime.WebHooks.Contains(scope)))
                    {
                        BCBinding binding = BCBinding.PK.Find(this, TYPE, hook.BindingID.Value);
                        BCEntity entity = BCEntity.PK.Find(this, TYPE, hook.BindingID.Value, info.EntityType);

                        if (binding == null || !(binding.IsActive ?? false) || entity == null || !(entity.IsActive ?? false)
                            || entity?.ImportRealTimeStatus != BCRealtimeStatusAttribute.Run || entity.Direction == BCSyncDirectionAttribute.Export)
                            continue;

                        Object obj = JsonConvert.DeserializeObject(data, info.ExternRealtime.WebHookType);
                        String id = obj?.ToString();
                        if (obj == null || id == null) continue;

                        toProcess[new RecordKey(entity.ConnectorType, entity.BindingID, entity.EntityType, id)]
                            = new RecordValue<String>((entity.RealTimeMode == BCSyncModeAttribute.PrepareAndProcess), (DateTime)created, message.Json);
                    }
                }
            }

            Dictionary<Int32, ConnectorOperation> toSync = new Dictionary<int, ConnectorOperation>();
            foreach (KeyValuePair<RecordKey, RecordValue<String>> pair in toProcess)
            {
                //Trigger Provider
                ConnectorOperation operation = new ConnectorOperation();
                operation.ConnectorType = pair.Key.ConnectorType;
                operation.Binding = pair.Key.BindingID.Value;
                operation.EntityType = pair.Key.EntityType;
                operation.PrepareMode = PrepareMode.None;
                operation.SyncMethod = SyncMode.Changed;

                Int32? syncID = null;
                EntityInfo info = this.GetEntities().FirstOrDefault(e => e.EntityType == pair.Key.EntityType);

                //Performance optimization - skip push if no value for that
                BCSyncStatus status = null;
                if (pair.Value.Timestamp != null)
                {
                    status = BCSyncStatus.ExternIDIndex.Find(this, operation.ConnectorType, operation.Binding, operation.EntityType, pair.Key.RecordID);
                    //Let the processor decide if deleted entries should resync - do not filter out deleted statuses
                    if (status != null && (status.LastOperation == BCSyncOperationAttribute.Skipped
                        || (status.ExternTS != null && pair.Value.Timestamp <= status.ExternTS)))
                        continue;
                }

                if (status == null || status.PendingSync == null || status.PendingSync == false)
                {
                    using (IProcessor graph = (IProcessor)PXGraph.CreateInstance(info.ProcessorType))
                    {
                        syncID = await graph.ProcessHook(this, operation, pair.Key.RecordID, pair.Value.Timestamp, pair.Value.ExternalInfo, status, cancellationToken);
                    }
                }
                else if (status.SyncInProcess == false) syncID = status.SyncID;

                if (syncID != null && pair.Value.AutoSync) toSync[syncID.Value] = operation;
            }
            if (toSync.Count > 0)
            {

                foreach (KeyValuePair<Int32, ConnectorOperation> pair in toSync)
                {
                    IConnector connector = ConnectorHelper.GetConnector(pair.Value.ConnectorType);
                    try
                    {
                        await connector.Process(pair.Value, new Int32?[] { pair.Key }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        connector.LogError(pair.Value.LogScope(pair.Key), ex);
                    }
                }
            }

        }

        public static WooRestClient GetRestClient(BCBindingWooCom binding)
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

        public List<Tuple<string, string, string>> GetExternalFields(string type, int? binding, string entity)
        {
            List<Tuple<string, string, string>> fieldsList = new List<Tuple<string, string, string>>();
            if (entity != BCEntitiesAttribute.Customer && entity != BCEntitiesAttribute.Address) return fieldsList;

            return fieldsList;
        }
    }
}
