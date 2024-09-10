using PX.Api.ContractBased.Models;
using PX.Commerce.Core;
using PX.Commerce.Core.API;
using PX.Commerce.Objects;
using PX.Data;
using PX.Objects.AR;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WooCommerceTest
{
    [BCProcessor(typeof(WooCommerceConnector), BCEntitiesAttribute.Customer, 
        BCCaptions.Customer, 20,
        IsInternal = false,
        Direction = SyncDirection.Bidirect,
        PrimaryDirection = SyncDirection.Import,
        PrimarySystem = PrimarySystem.Extern,
        PrimaryGraph = typeof(PX.Objects.AR.CustomerMaint),
        ExternTypes = new Type[] { typeof(CustomerData) },
        LocalTypes = new Type[] { typeof(PX.Commerce.Core.API.Customer) },
        AcumaticaPrimaryType = typeof(PX.Objects.AR.Customer),
        AcumaticaPrimarySelect = typeof(PX.Objects.AR.Customer.acctCD),
        URL = "user-edit.php?user_id={0}"
    )]
    [BCProcessorRealtime(
        PushSupported = true,
        PushSources = new String[] { "BC-PUSH-Customers" },
        PushDestination = WCCaptions.PushDestination,
        HookSupported = true,
        WebHookType = typeof(CustomerData),
        WebHooks = new String[]
        {
            "customer.created",
            "customer.updated",
            "customer.deleted"
        }
    )]
    public class WooCustomerProcessor : 
        BCProcessorSingleBase<WooCustomerProcessor, 
            WooCustomerEntityBucket, MappedCustomer>, IProcessor
    {
        public RestClient client;
        protected CustomerDataProvider customerDataProvider;

        //protected List<Country> countries;
        public CommerceHelper helper = 
            PXGraph.CreateInstance<CommerceHelper>();

        #region Import
        public override async Task MapBucketImport(
            WooCustomerEntityBucket bucket, IMappedEntity existing, 
            CancellationToken cancellationToken = default)
        {
            MappedCustomer customerObj = bucket.Customer;

            PX.Commerce.Core.API.Customer customerImpl = customerObj.Local = 
                new PX.Commerce.Core.API.Customer();
            customerImpl.Custom = GetCustomFieldsForImport();

            customerImpl.CustomerName = 
                GetBillingCustomerName(customerObj.Extern).ValueField();
            customerImpl.AccountRef = APIHelper.ReferenceMake(
                customerObj.Extern.Id, GetBinding().BindingName).ValueField();
            customerImpl.CustomerClass = customerObj.LocalID == null || 
                existing?.Local == null ?
                GetBindingExt<BCBindingExt>().CustomerClassID?.ValueField() : 
                    null;

            //Primary Contact
            customerImpl.PrimaryContact = 
                GetPrimaryContact(customerObj.Extern);

            customerImpl.MainContact = SetBillingContact(customerObj.Extern);

            customerImpl.ShippingAddressOverride = true.ValueField();
            customerImpl.ShippingContactOverride = true.ValueField();
            customerImpl.ShippingContact = 
                SetShippingContact(customerObj.Extern);

            BCBindingExt bindingExt = GetBindingExt<BCBindingExt>();
            if (bindingExt.CustomerClassID != null)
            {
                CustomerClass customerClass = PXSelect<CustomerClass, 
                    Where<CustomerClass.customerClassID, 
                        Equal<Required<CustomerClass.customerClassID>>>>.
                            Select(this, bindingExt.CustomerClassID);
                if (customerClass != null)
                {
                    customerClass.ShipVia.ValueField();

                }
            }

        }

        public virtual Contact GetPrimaryContact(CustomerData customer)
        {
            var contact = new Contact();
            contact.FirstName = !string.IsNullOrWhiteSpace(customer.FirstName) 
                && string.IsNullOrWhiteSpace(customer.LastName) ? 
                string.Empty.ValueField() : customer.FirstName.ValueField();
            contact.LastName = !string.IsNullOrWhiteSpace(customer.LastName) ? 
                customer.LastName.ValueField() :
                !string.IsNullOrWhiteSpace(customer.FirstName) && 
                string.IsNullOrWhiteSpace(customer.LastName) ?
                customer.FirstName.ValueField() : 
                customer.Username.ValueField();
            contact.Email = customer.Email.ValueField();

            return contact;
        }

        public virtual Contact SetBillingContact(CustomerData customerObj)
        {
            Contact contactImpl = new Contact();
            contactImpl.DisplayName = 
                GetBillingCustomerName(customerObj).ValueField();
            contactImpl.Attention = 
            $"{customerObj.Billing?.FirstName} {customerObj.Billing?.LastName}"
            .ValueField(); ;
            contactImpl.FullName = GetBillingCustomerName(customerObj)
            .ValueField();
            contactImpl.FirstName = customerObj.Billing?.FirstName
            .ValueField();
            contactImpl.LastName = customerObj.Billing.LastName.ValueField();
            contactImpl.Email = customerObj.Billing.Email.ValueField();
            contactImpl.Address = MapAddress(customerObj.Billing);
            contactImpl.Active = true.ValueField();
            contactImpl.Phone1 = customerObj.Billing?.Phone.ValueField();
            contactImpl.Phone1Type = PhoneTypes.Business1.ValueField();

            return contactImpl;
        }

        public virtual Contact SetShippingContact(CustomerData customerObj)
        {
            Contact contactImpl = new Contact();
            contactImpl.DisplayName = GetShippingCustomerName(customerObj)
            .ValueField();
            contactImpl.Attention = 
            $"{customerObj.Shipping?.FirstName} {customerObj.Shipping?.LastName}"
            .ValueField();
            contactImpl.FullName = GetShippingCustomerName(customerObj)
            .ValueField();
            contactImpl.FirstName = customerObj.Shipping?.FirstName
            .ValueField();
            contactImpl.LastName = customerObj.Shipping.LastName.ValueField();
            contactImpl.Address = MapAddress(customerObj.Shipping);
            contactImpl.Active = true.ValueField();

            return contactImpl;
        }

        public virtual string GetBillingCustomerName(CustomerData data)
        {
            return !string.IsNullOrWhiteSpace(data.Billing.Company)
                ? data.Billing.Company
                : string.IsNullOrWhiteSpace(
                $"{data.Billing?.FirstName} {data.Billing?.LastName}") 
                ? data.Username : 
                $"{data.Billing?.FirstName} {data.Billing?.LastName}";
        }

        public virtual string GetShippingCustomerName(CustomerData data)
        {
            return !string.IsNullOrWhiteSpace(data.Shipping?.Company)
                ? data.Shipping.Company
                : $"{data.Shipping?.FirstName} {data.Shipping?.LastName}";
        }

        public virtual Address MapAddress(CustomerAddressData addressObj)
        {
            var address = new Address();
            address.AddressLine1 = addressObj.Address1.ValueField();
            address.AddressLine2 = addressObj.Address2.ValueField();
            address.City = addressObj.City.ValueField();
            address.PostalCode = addressObj.PostalCode.ValueField();
            if (!string.IsNullOrWhiteSpace(addressObj.Country))
            {
                var selectedCountry = addressObj.Country;
                if (selectedCountry != null)
                {
                    address.Country = addressObj.Country.ValueField();
                    if (!string.IsNullOrWhiteSpace(addressObj.State))
                    {
                        address.State = addressObj.State.ValueField();
                    }
                }
            }

            return address;
        }

        public override async Task<PullSimilarResult<MappedCustomer>> 
            PullSimilar(IExternEntity entity, 
            CancellationToken cancellationToken = default)
        {
            var uniqueField = string.IsNullOrWhiteSpace(
              ((CustomerData)entity)?.Billing.Email) ? 
              ((CustomerData)entity)?.Email : 
              ((CustomerData)entity)?.Billing.Email;
            if (uniqueField == null) return null;

            List<MappedCustomer> result = new List<MappedCustomer>();
            foreach (PX.Objects.AR.Customer item in 
                helper.CustomerByEmail.Select(uniqueField))
            {
                PX.Commerce.Core.API.Customer data = 
                    new PX.Commerce.Core.API.Customer() { 
                      SyncID = item.NoteID, 
                      SyncTime = item.LastModifiedDateTime };
                result.Add(
                    new MappedCustomer(data, data.SyncID, data.SyncTime));
            }

            if (result == null || result?.Count == 0) return null;

            return new PullSimilarResult<MappedCustomer>() { 
                UniqueField = uniqueField, Entities = result };
        }

        public override async Task<PullSimilarResult<MappedCustomer>> 
            PullSimilar(ILocalEntity entity, 
            CancellationToken cancellationToken = default)
        {
            var uniqueField = ((PX.Commerce.Core.API.Customer)entity)?.
                MainContact?.Email?.Value;
            if (uniqueField == null) return null;
            IEnumerable<CustomerData> datas = customerDataProvider.GetAll();
            if (datas == null) return null;

            return new PullSimilarResult<MappedCustomer>() { 
                UniqueField = uniqueField, 
                Entities = datas.Select(data => new MappedCustomer(
                data, data.Id.ToString(), data.DateModified.ToDate())) };
        }

        public override async Task SaveBucketImport(
            WooCustomerEntityBucket bucket, IMappedEntity existing, 
            string operation, CancellationToken cancellationToken = default)
        {
            MappedCustomer obj = bucket.Customer;

            PX.Commerce.Core.API.Customer impl = cbapi.Put(obj.Local, 
                obj.LocalID);

            obj.AddLocal(impl, impl.SyncID, impl.SyncTime);
            UpdateStatus(obj, operation);
        }
        #endregion

        #region Export

        public override async Task FetchBucketsForExport(
            DateTime? minDateTime, DateTime? maxDateTime, 
            PXFilterRow[] filters, CancellationToken cancel = default)
        {
            IEnumerable<PX.Commerce.Core.API.Customer> impls = 
                cbapi.GetAll<PX.Commerce.Core.API.Customer>(
                new PX.Commerce.Core.API.Customer { 
                    CustomerID = new StringReturn() },
                minDateTime, maxDateTime, filters);

            int countNum = 0;
            List<IMappedEntity> mappedList = new List<IMappedEntity>();
            foreach (PX.Commerce.Core.API.Customer impl in impls)
            {
                IMappedEntity obj = new MappedCustomer(impl, impl.SyncID, 
                    impl.SyncTime);

                mappedList.Add(obj);
                countNum++;
                if (countNum % BatchFetchCount == 0)
                {
                    ProcessMappedListForExport(mappedList);
                }
            }
            if (mappedList.Any())
            {
                ProcessMappedListForExport(mappedList);
            }
        }

        public override async Task MapBucketExport(
            WooCustomerEntityBucket bucket, IMappedEntity existing, 
            CancellationToken cancel = default)
        {
            MappedCustomer customerObj = bucket.Customer;

            PX.Commerce.Core.API.Customer customerImpl = customerObj.Local;
            Contact contactImpl = customerImpl.MainContact;
            Contact primaryContact = customerImpl.PrimaryContact;
            Address addressImpl = contactImpl.Address;
            CustomerData customerData = customerObj.Extern = 
                new CustomerData();

            //Customer
            customerData.Id = customerObj.ExternID.ToInt();

            //Contact	
            // Use primary contact firstname and last name 
            // as Customer's firstname and last name 
            // if primarycontact is present else use Customername
            customerData.FirstName = primaryContact?.FirstName?.Value ?? 
                primaryContact?.LastName?.Value ?? 
                customerImpl.CustomerName?.Value.FieldsSplit(
                0, customerImpl.CustomerName?.Value);
            customerData.LastName = primaryContact?.LastName?.Value ?? 
                primaryContact?.FirstName?.Value ?? 
                customerImpl.CustomerName?.Value.FieldsSplit(
                1, customerImpl.CustomerName?.Value);
            customerData.Email = contactImpl.Email?.Value;

            if (!string.IsNullOrEmpty(customerImpl.PriceClassID?.Value))
            {
                BCSyncStatus status = PXSelectJoin<BCSyncStatus,
                    LeftJoin<PX.Objects.AR.ARPriceClass, 
                        On<BCSyncStatus.localID, 
                            Equal<PX.Objects.AR.ARPriceClass.noteID>>>,
                    Where<BCSyncStatus.connectorType, 
                        Equal<Current<BCEntity.connectorType>>,
                        And<BCSyncStatus.bindingID, 
                            Equal<Current<BCEntity.bindingID>>,
                        And<BCSyncStatus.entityType, 
                            Equal<Required<BCEntity.entityType>>,
                        And<PX.Objects.AR.ARPriceClass.priceClassID, 
                            Equal<Required<
                                PX.Objects.AR.ARPriceClass.priceClassID>>>>>>>.
                    Select(this, BCEntitiesAttribute.CustomerPriceClass, 
                        customerImpl.PriceClassID?.Value);
                if (GetEntity(BCEntitiesAttribute.CustomerPriceClass)?.
                    IsActive == true)
                {
                    if (status?.ExternID == null) throw new PXException(
                        BCMessages.PriceClassNotSyncronizedForItem, 
                        customerImpl.PriceClassID?.Value);
                }
            }
        }

        public override async Task SaveBucketExport(
            WooCustomerEntityBucket bucket, IMappedEntity existing, 
            string operation, CancellationToken cancel = default)
        {
            MappedCustomer obj = bucket.Customer;

            //Customer
            CustomerData customerData = null;
            obj.AddExtern(customerData, customerData.Id?.ToString(), 
                customerData.LastName, customerData.DateCreatedUT.ToDate());

            UpdateStatus(obj, operation);

            #region Update ExternalRef
            string externalRef = APIHelper.ReferenceMake(
                customerData.Id?.ToString(), GetBinding().BindingName);

            string[] keys = obj.Local?.AccountRef?.Value?.Split(';');
            if (keys?.Contains(externalRef) != true)
            {
                if (!string.IsNullOrEmpty(obj.Local?.AccountRef?.Value))
                    externalRef = new object[] { obj.Local?.AccountRef?.Value, 
                        externalRef }.KeyCombine();

                if (externalRef.Length < 50 && obj.Local.SyncID != null)
                    PXDatabase.Update<PX.Objects.CR.BAccount>(
                        new PXDataFieldAssign(
                        typeof(PX.Objects.CR.BAccount.acctReferenceNbr).Name, 
                        PXDbType.NVarChar, externalRef),
                        new PXDataFieldRestrict(
                        typeof(PX.Objects.CR.BAccount.noteID).Name, 
                        PXDbType.UniqueIdentifier, obj.Local.NoteID?.Value)
                    );
            }
            #endregion

        }
        #endregion

        #region Pull
        public override async Task<MappedCustomer> PullEntity(Guid? localID, 
            Dictionary<string, object> externalInfo, 
                CancellationToken cancellationToken = default)
        {
            PX.Commerce.Core.API.Customer impl = 
                cbapi.GetByID<PX.Commerce.Core.API.Customer>(localID);
            if (impl == null) return null;

            MappedCustomer obj = new MappedCustomer(impl, impl.SyncID, 
                impl.SyncTime);

            return obj;
        }
        public override async Task<MappedCustomer> PullEntity(string externID, 
            string externalInfo, CancellationToken cancellationToken = default)
        {
            client = WooCommerceConnector.GetRestClient(
                GetBindingExt<BCBindingWooCommerce>());
            customerDataProvider = new CustomerDataProvider(client);
            CustomerData data = customerDataProvider.GetAll().FirstOrDefault();
            if (data == null) return null;

            MappedCustomer obj = new MappedCustomer(data, data.Id?.ToString(), 
                data.DateModified.ToDate());
            return obj;
        }

        public override async Task FetchBucketsForImport(DateTime? minDateTime, 
            DateTime? maxDateTime, PXFilterRow[] filters, 
            CancellationToken cancellationToken = default)
        {
            IEnumerable<CustomerData> customers = customerDataProvider.GetAll();

            foreach (CustomerData data in customers)
            {
                WooCustomerEntityBucket bucket = CreateBucket();

                MappedCustomer mapped = bucket.Customer = bucket.Customer.Set(
                    data, data.Id.ToString(), data.LastName, 
                    data.DateModified.ToDate());
                EnsureStatus(mapped, SyncDirection.Import);
            }
        }

        public override async Task<EntityStatus> GetBucketForImport(
            WooCustomerEntityBucket bucket, BCSyncStatus syncstatus, 
            CancellationToken cancel = default)
        {
            CustomerData data = client.Get<CustomerData>(
                new RestRequest(string.Format("/customers/{0}", 
                syncstatus.ExternID)));

            MappedCustomer obj = bucket.Customer = bucket.Customer.Set(data, 
                data.Id.ToString(), data.LastName, data.DateModified.ToDate());
            EntityStatus status = EnsureStatus(obj, SyncDirection.Import);

            return status;
        }

        public override async Task<EntityStatus> GetBucketForExport(
            WooCustomerEntityBucket bucket, BCSyncStatus bcstatus, 
            CancellationToken cancellationToken = default)
        {
            PX.Commerce.Core.API.Customer impl = cbapi.GetByID<
                PX.Commerce.Core.API.Customer>(bcstatus.LocalID, 
                GetCustomFieldsForExport());
            if (impl == null) return EntityStatus.None;

            bucket.Customer = bucket.Customer.Set(impl, impl.SyncID, 
                impl.SyncTime);
            EntityStatus status = EnsureStatus(bucket.Customer, 
                SyncDirection.Export);

            return status;
        }
        #endregion
    }
    public static class PhoneTypes
    {
        public static string Business1 = "B1";
    }
}
