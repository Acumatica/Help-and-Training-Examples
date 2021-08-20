using System;
using System.Collections.Generic;
using System.ComponentModel;
using PX.Commerce.Core;
using Newtonsoft.Json;

namespace WooCommerceTest
{
    [Description(WCCaptions.Customer)]
    public class CustomerData : BCAPIEntity, IWooEntity
    {
        [JsonProperty("id")]
        [CommerceDescription(WCCaptions.ID, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public int? Id { get; set; }

        [JsonProperty("date_created")]
        public DateTime? DateCreatedUT { get; set; }

        [Description(WCCaptions.DateCreatedUT)]
        [ShouldNotSerialize]
        public virtual DateTime? CreatedDateTime
        {
            get
            {
                return DateCreatedUT != null ? (DateTime)DateCreatedUT.ToDate() : default;
            }
        }

        [JsonProperty("date_modified_gmt")]
        public DateTime? DateModified { get; set; }

        [Description(WCCaptions.DateModifiedUT)]
        [ShouldNotSerialize]
        public virtual DateTime? ModifiedDateTime
        {
            get
            {
                return DateModified != null ? (DateTime)DateModified.ToDate() : default;
            }
        }

        [JsonProperty("email")]
        [CommerceDescription(WCCaptions.Email, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        [CommerceDescription(WCCaptions.FirstName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        [CommerceDescription(WCCaptions.LastName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        [ValidateRequired()]
        public string LastName { get; set; }

        [JsonProperty("role")]
        [CommerceDescription(WCCaptions.Role, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public string Role { get; set; }

        [JsonProperty("username")]
        [CommerceDescription(WCCaptions.UserName, FieldFilterStatus.Skipped, FieldMappingStatus.Import)]
        public string Username { get; set; }

        [JsonProperty("billing")]
        public CustomerAddressData Billing { get; set; }

        [JsonProperty("shipping")]
        public CustomerAddressData Shipping { get; set; }

        [JsonProperty("is_paying_customer")]
        public bool? IsPayingCustomer { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("meta_data")]
        public IEnumerable<object> MetaData { get; set; }


    }
}
