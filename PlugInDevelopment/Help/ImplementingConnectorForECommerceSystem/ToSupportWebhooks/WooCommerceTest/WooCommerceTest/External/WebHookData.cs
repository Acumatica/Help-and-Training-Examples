using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    [CommerceDescription(WCCaptions.Webhook)]
    public class WebHookData : BCAPIEntity, IWooEntity
    {
        [JsonProperty("id")]
        public virtual int? Id { get; set; }

        public virtual string ClientID { get; set; }

        [JsonProperty("secret")]
        public virtual string StoreHash { get; set; }

        [JsonProperty("topic")]
        public virtual string Scope { get; set; }

        [JsonProperty("delivery_url")]
        public virtual string Destination { get; set; }

        [JsonProperty("active")]
        public virtual string Active { get; set; }

        [JsonIgnore]
        public virtual DateTime? DateCreated { get; set; }

        [JsonProperty("date_created")]
        public DateTime? DateCreatedUT { get; set; }

        [CommerceDescription(WCCaptions.DateCreatedUT)]
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

        [CommerceDescription(WCCaptions.DateModifiedUT)]
        [ShouldNotSerialize]
        public virtual DateTime? ModifiedDateTime
        {
            get
            {
                return DateModified != null ? (DateTime)DateModified.ToDate() : default;
            }
        }

        [JsonProperty("headers")]
        public virtual Dictionary<String, String> Headers { get; set; }

        //Conditional Serialization
        public bool ShouldSerializeId()
        {
            return false;
        }
        public bool ShouldSerializeDateCreatedUT()
        {
            return false;
        }
        public bool ShouldSerializeDateModifiedUT()
        {
            return false;
        }
    }
}
