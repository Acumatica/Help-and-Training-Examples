using System;
using System.ComponentModel;
using PX.Commerce.Core;
using Newtonsoft.Json;

namespace WooCommerceTest
{
    public class CustomerAddressData : BCAPIEntity, IEquatable<CustomerAddressData>
    {
        [JsonProperty("customer_id")]
        [CommerceDescription(WCCaptions.CustomerId, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public virtual int? CustomerId { get; set; }

        [JsonProperty("first_name")]
        [CommerceDescription(WCCaptions.FirstName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired()]
        public virtual string FirstName { get; set; }

        [JsonProperty("last_name")]
        [CommerceDescription(WCCaptions.LastName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public virtual string LastName { get; set; }

        [JsonProperty("company")]
        [CommerceDescription(WCCaptions.CompanyName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public virtual string Company { get; set; }

        [JsonProperty("address_1")]
        [CommerceDescription(WCCaptions.AddressLine1, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired(AutoDefault = true)]
        public string Address1 { get; set; }

        [JsonProperty("address_2")]
        [CommerceDescription(WCCaptions.AddressLine2, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        [CommerceDescription(WCCaptions.City, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired(AutoDefault = true)]
        public virtual string City { get; set; }

        [JsonProperty("postcode")]
        [CommerceDescription(WCCaptions.PostalCode, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired(AutoDefault = true)]
        public virtual string PostalCode { get; set; }

        [JsonProperty("country")]
        [CommerceDescription(WCCaptions.Country, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired()]
        public virtual string Country { get; set; }

        [JsonProperty("state")]
        [Description(WCCaptions.State)]
        [ValidateRequired(AutoDefault = true)]
        public virtual string State { get; set; }

        [JsonProperty("phone")]
        [Description(WCCaptions.Phone)]
        [ValidateRequired(AutoDefault = true)]
        public virtual string Phone { get; set; }

        [JsonProperty("email")]
        [Description(WCCaptions.Email)]
        [ValidateRequired(AutoDefault = true)]
        public virtual string Email { get; set; }

        public bool Equals(CustomerAddressData newObject)
        {
            var oldObject = this;
            if (ReferenceEquals(newObject, oldObject)) return true;
            if (newObject == null || oldObject == null) return false;

            if (newObject.GetType() != oldObject.GetType()) return false;

            var result = true;

            foreach (var property in newObject.GetType().GetProperties())
            {
                var objValue = property.GetValue(newObject);
                var anotherValue = property.GetValue(oldObject);
                if (objValue == null || anotherValue == null)
                    result = objValue == anotherValue;
                else if (!objValue.Equals(anotherValue)) result = false;

                if (!result && !(property.Name == nameof(newObject.Phone) || property.Name == nameof(newObject.Email)))
                    return result;
            }

            return result;
        }
    }
}
