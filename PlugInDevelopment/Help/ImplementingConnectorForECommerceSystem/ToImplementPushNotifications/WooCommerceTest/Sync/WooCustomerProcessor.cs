﻿using PX.Api.ContractBased.Models;
using PX.Commerce.Core;
using PX.Commerce.Core.API;
using PX.Commerce.Objects;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WooCommerceTest
{
    [BCProcessor(typeof(WooCommerceConnector), BCEntitiesAttribute.Customer, BCCaptions.Customer,
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
    [BCProcessorRealtime(PushSupported = true, HookSupported = false,
        PushSources = new String[] { "BC-PUSH-Customers" }
    )]
    public class WooCustomerProcessor : BCProcessorSingleBase<WooCustomerProcessor, WooCustomerEntityBucket, MappedCustomer>, IProcessor
    {
        public WooRestClient client;
        protected CustomerDataProvider customerDataProvider;

        //protected List<Country> countries;
        public CommerceHelper helper = PXGraph.CreateInstance<CommerceHelper>();


        #region Constructor
        public override void Initialise(IConnector iconnector, ConnectorOperation operation)
        {
            base.Initialise(iconnector, operation);
            client = WooCommerceConnector.GetRestClient(GetBindingExt<BCBindingWooCommerce>());
            customerDataProvider = new CustomerDataProvider(client);
        }
        #endregion

        #region Import
        public override void MapBucketImport(WooCustomerEntityBucket bucket, IMappedEntity existing)
        {
            MappedCustomer customerObj = bucket.Customer;

            PX.Commerce.Core.API.Customer customerImpl = customerObj.Local = new PX.Commerce.Core.API.Customer();
            customerImpl.Custom = GetCustomFieldsForImport();

            customerImpl.CustomerName = GetBillingCustomerName(customerObj.Extern).ValueField();
            customerImpl.AccountRef = APIHelper.ReferenceMake(customerObj.Extern.Id, GetBinding().BindingName).ValueField();
            customerImpl.CustomerClass = customerObj.LocalID == null || existing?.Local == null ?
                GetBindingExt<BCBindingExt>().CustomerClassID?.ValueField() : null;

            //Primary Contact
            customerImpl.PrimaryContact = GetPrimaryContact(customerObj.Extern);

            customerImpl.MainContact = SetBillingContact(customerObj.Extern);

            customerImpl.ShippingAddressOverride = true.ValueField();
            customerImpl.ShippingContactOverride = true.ValueField();
            customerImpl.ShippingContact = SetShippingContact(customerObj.Extern);

            BCBindingExt bindingExt = GetBindingExt<BCBindingExt>();
            if (bindingExt.CustomerClassID != null)
            {
                CustomerClass customerClass = PXSelect<CustomerClass, Where<CustomerClass.customerClassID, Equal<Required<CustomerClass.customerClassID>>>>.Select(this, bindingExt.CustomerClassID);
                if (customerClass != null)
                {
                    customerClass.ShipVia.ValueField();

                }
            }

        }

        public virtual Contact GetPrimaryContact(CustomerData customer)
        {
            var contact = new Contact();
            contact.FirstName = !string.IsNullOrWhiteSpace(customer.FirstName) && string.IsNullOrWhiteSpace(customer.LastName) ? string.Empty.ValueField() : customer.FirstName.ValueField();
            contact.LastName = !string.IsNullOrWhiteSpace(customer.LastName) ? customer.LastName.ValueField() :
                !string.IsNullOrWhiteSpace(customer.FirstName) && string.IsNullOrWhiteSpace(customer.LastName) ?
                customer.FirstName.ValueField() : customer.Username.ValueField();
            contact.Email = customer.Email.ValueField();

            return contact;
        }

        public virtual Contact SetBillingContact(CustomerData customerObj)
        {
            Contact contactImpl = new Contact();
            contactImpl.DisplayName = GetBillingCustomerName(customerObj).ValueField();
            contactImpl.Attention = $"{customerObj.Billing?.FirstName} {customerObj.Billing?.LastName}".ValueField(); ;
            contactImpl.FullName = GetBillingCustomerName(customerObj).ValueField();
            contactImpl.FirstName = customerObj.Billing?.FirstName.ValueField();
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
            contactImpl.DisplayName = GetShippingCustomerName(customerObj).ValueField();
            contactImpl.Attention = $"{customerObj.Shipping?.FirstName} {customerObj.Shipping?.LastName}".ValueField();
            contactImpl.FullName = GetShippingCustomerName(customerObj).ValueField();
            contactImpl.FirstName = customerObj.Shipping?.FirstName.ValueField();
            contactImpl.LastName = customerObj.Shipping.LastName.ValueField();
            contactImpl.Address = MapAddress(customerObj.Shipping);
            contactImpl.Active = true.ValueField();

            return contactImpl;
        }

        public virtual string GetBillingCustomerName(CustomerData data)
        {
            return !string.IsNullOrWhiteSpace(data.Billing.Company)
                ? data.Billing.Company
                : string.IsNullOrWhiteSpace($"{data.Billing?.FirstName} {data.Billing?.LastName}") ? data.Username : $"{data.Billing?.FirstName} {data.Billing?.LastName}";
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

        public override IEnumerable<MappedCustomer> PullSimilar(IExternEntity entity, out string uniqueField)
        {
            uniqueField = string.IsNullOrWhiteSpace(((CustomerData)entity)?.Billing.Email) ? ((CustomerData)entity)?.Email : ((CustomerData)entity)?.Billing.Email;
            if (uniqueField == null) return null;

            List<MappedCustomer> result = new List<MappedCustomer>();
            foreach (PX.Objects.AR.Customer item in helper.CustomerByEmail.Select(uniqueField))
            {
                PX.Commerce.Core.API.Customer data = new PX.Commerce.Core.API.Customer() { SyncID = item.NoteID, SyncTime = item.LastModifiedDateTime };
                result.Add(new MappedCustomer(data, data.SyncID, data.SyncTime));
            }

            if (result == null || result?.Count == 0) return null;

            return result;
        }

        public override IEnumerable<MappedCustomer> PullSimilar(ILocalEntity entity, out string uniqueField)
        {
            uniqueField = ((PX.Commerce.Core.API.Customer)entity)?.MainContact?.Email?.Value;
            if (uniqueField == null) return null;
            /*FilterCustomers filter = new FilterCustomers { Email = uniqueField };
            IEnumerable<CustomerData> datas = customerDataProvider.GetAll(filter);*/
            IEnumerable<CustomerData> datas = customerDataProvider.GetAll(null);
            if (datas == null) return null;

            return datas.Select(data => new MappedCustomer(data, data.Id.ToString(), data.DateModified.ToDate()));
        }

        public override void SaveBucketImport(WooCustomerEntityBucket bucket, IMappedEntity existing, string operation)
        {
            MappedCustomer obj = bucket.Customer;

            PX.Commerce.Core.API.Customer impl = cbapi.Put(obj.Local, obj.LocalID);

            obj.AddLocal(impl, impl.SyncID, impl.SyncTime);
            UpdateStatus(obj, operation);
        }
        #endregion

        #region Export

        public override void FetchBucketsForExport(DateTime? minDateTime, DateTime? maxDateTime, PXFilterRow[] filters)
        {
            IEnumerable<PX.Commerce.Core.API.Customer> impls = cbapi.GetAll<PX.Commerce.Core.API.Customer>(
                new PX.Commerce.Core.API.Customer { CustomerID = new StringReturn() },
                minDateTime, maxDateTime, filters);

            int countNum = 0;
            List<IMappedEntity> mappedList = new List<IMappedEntity>();
            foreach (PX.Commerce.Core.API.Customer impl in impls)
            {
                IMappedEntity obj = new MappedCustomer(impl, impl.SyncID, impl.SyncTime);

                mappedList.Add(obj);
                countNum++;
                if (countNum % BatchFetchCount == 0)
                {
                    ProcessMappedListForExport(ref mappedList);
                }
            }
            if (mappedList.Any())
            {
                ProcessMappedListForExport(ref mappedList);
            }
        }

        public override void MapBucketExport(WooCustomerEntityBucket bucket, IMappedEntity existing)
        {
            MappedCustomer customerObj = bucket.Customer;

            PX.Commerce.Core.API.Customer customerImpl = customerObj.Local;
            Contact contactImpl = customerImpl.MainContact;
            Contact primaryContact = customerImpl.PrimaryContact;
            Address addressImpl = contactImpl.Address;
            CustomerData customerData = customerObj.Extern = new CustomerData();

            //Customer
            customerData.Id = customerObj.ExternID.ToInt();

            //Contact	
            // Use primary contact firstname and last name as Customer's firstname and last name if primarycontact is present else use Customername
            customerData.FirstName = primaryContact?.FirstName?.Value ?? primaryContact?.LastName?.Value ?? customerImpl.CustomerName?.Value.FieldsSplit(0, customerImpl.CustomerName?.Value);
            customerData.LastName = primaryContact?.LastName?.Value ?? primaryContact?.FirstName?.Value ?? customerImpl.CustomerName?.Value.FieldsSplit(1, customerImpl.CustomerName?.Value);
            customerData.Email = contactImpl.Email?.Value;

            if (!string.IsNullOrEmpty(customerImpl.PriceClassID?.Value))
            {
                BCSyncStatus status = PXSelectJoin<BCSyncStatus,
                    LeftJoin<PX.Objects.AR.ARPriceClass, On<BCSyncStatus.localID, Equal<PX.Objects.AR.ARPriceClass.noteID>>>,
                    Where<BCSyncStatus.connectorType, Equal<Current<BCEntity.connectorType>>,
                        And<BCSyncStatus.bindingID, Equal<Current<BCEntity.bindingID>>,
                        And<BCSyncStatus.entityType, Equal<Required<BCEntity.entityType>>,
                        And<PX.Objects.AR.ARPriceClass.priceClassID, Equal<Required<PX.Objects.AR.ARPriceClass.priceClassID>>>>>>>.Select(this, BCEntitiesAttribute.CustomerPriceClass, customerImpl.PriceClassID?.Value);
                if (GetEntity(BCEntitiesAttribute.CustomerPriceClass)?.IsActive == true)
                {
                    if (status?.ExternID == null) throw new PXException(BCMessages.PriceClassNotSyncronizedForItem, customerImpl.PriceClassID?.Value);
                }
            }
        }

        public override void SaveBucketExport(WooCustomerEntityBucket bucket, IMappedEntity existing, string operation)
        {
            MappedCustomer obj = bucket.Customer;

            //Customer
            CustomerData customerData = null;
            /*IList<CustomerFormFieldData> formFields = obj.Extern.FormFields;
            //If customer form fields are not empty, it could cause an error		
            obj.Extern.FormFields = null;
            if (obj.ExternID == null || existing == null)
                customerData = customerDataProviderV3.Create(obj.Extern);
            else
                customerData = customerDataProviderV3.Update(obj.Extern);
            if (formFields?.Count > 0 && customerData.Id != null)
            {
                formFields.All(x => { x.CustomerId = customerData.Id; x.AddressId = null; x.Value = x.Value ?? string.Empty; return true; });
                customerData.FormFields = customerFormFieldRestDataProvider.UpdateAll((List<CustomerFormFieldData>)formFields);
            }*/
            obj.AddExtern(customerData, customerData.Id?.ToString(), customerData.DateCreatedUT.ToDate());

            UpdateStatus(obj, operation);

            #region Update ExternalRef
            string externalRef = APIHelper.ReferenceMake(customerData.Id?.ToString(), GetBinding().BindingName);

            string[] keys = obj.Local?.AccountRef?.Value?.Split(';');
            if (keys?.Contains(externalRef) != true)
            {
                if (!string.IsNullOrEmpty(obj.Local?.AccountRef?.Value))
                    externalRef = new object[] { obj.Local?.AccountRef?.Value, externalRef }.KeyCombine();

                if (externalRef.Length < 50 && obj.Local.SyncID != null)
                    PXDatabase.Update<PX.Objects.CR.BAccount>(
                                  new PXDataFieldAssign(typeof(PX.Objects.CR.BAccount.acctReferenceNbr).Name, PXDbType.NVarChar, externalRef),
                                  new PXDataFieldRestrict(typeof(PX.Objects.CR.BAccount.noteID).Name, PXDbType.UniqueIdentifier, obj.Local.NoteID?.Value)
                                  );
            }
            #endregion

        }
        #endregion

        #region Pull
        public override MappedCustomer PullEntity(Guid? localID, Dictionary<string, object> fields)
        {
            PX.Commerce.Core.API.Customer impl = cbapi.GetByID<PX.Commerce.Core.API.Customer>(localID);
            if (impl == null) return null;

            MappedCustomer obj = new MappedCustomer(impl, impl.SyncID, impl.SyncTime);

            return obj;
        }
        public override MappedCustomer PullEntity(string externID, string jsonObject)
        {
            client = WooCommerceConnector.GetRestClient(GetBindingExt<BCBindingWooCommerce>());
            customerDataProvider = new CustomerDataProvider(client);
            CustomerData data = customerDataProvider.GetAll(null).FirstOrDefault();
            if (data == null) return null;

            MappedCustomer obj = new MappedCustomer(data, data.Id?.ToString(), data.DateModified.ToDate());
            return obj;
        }

        public override void FetchBucketsForImport(DateTime? minDateTime, DateTime? maxDateTime, PXFilterRow[] filters)
        {
            /*FilterProductCategories filter = new FilterProductCategories();
            if (minDateTime != null)
                filter.CreatedAfter = minDateTime;
            if (maxDateTime != null)
                filter.CreatedAfter = maxDateTime;

            IEnumerable<CustomerData> customers = customerDataProvider.GetAll(filter);*/
            IEnumerable<CustomerData> customers = customerDataProvider.GetAll(null);

            foreach (CustomerData data in customers)
            {
                WooCustomerEntityBucket bucket = CreateBucket();

                MappedCustomer mapped = bucket.Customer = bucket.Customer.Set(data, data.Id.ToString(), data.DateModified.ToDate());
                EnsureStatus(mapped, SyncDirection.Import);
            }
        }

        public override EntityStatus GetBucketForImport(WooCustomerEntityBucket bucket, BCSyncStatus syncstatus)
        {
            CustomerData data = client.Get<CustomerData>(
                string.Format("/customers/{0}", syncstatus.ExternID));

            MappedCustomer obj = bucket.Customer = bucket.Customer.Set(data, data.Id.ToString(), data.DateModified.ToDate());
            EntityStatus status = EnsureStatus(obj, SyncDirection.Import);

            return status;
        }

        public override EntityStatus GetBucketForExport(WooCustomerEntityBucket bucket, BCSyncStatus bcstatus)
        {
            PX.Commerce.Core.API.Customer impl = cbapi.GetByID<PX.Commerce.Core.API.Customer>(bcstatus.LocalID, GetCustomFieldsForExport());
            if (impl == null) return EntityStatus.None;

            bucket.Customer = bucket.Customer.Set(impl, impl.SyncID, impl.SyncTime);
            EntityStatus status = EnsureStatus(bucket.Customer, SyncDirection.Export);

            return status;
        }
        #endregion
    }
    public static class PhoneTypes
    {
        public static string Business1 = "B1";
    }
}
