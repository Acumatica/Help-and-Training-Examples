using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    public class WebHookMessage
    {
        [JsonProperty("topic")]
        public virtual string Scope { get; set; }

        [JsonProperty("store_id")]
        public virtual int? StoreID { get; set; }

        [JsonProperty("hash")]
        public virtual string Hash { get; set; }

        [JsonProperty("producer")]
        public virtual string Producer { get; set; }

        [JsonProperty("date_created")]
        public virtual string DateCreatedUT { get; set; }

        [JsonIgnore]
        public virtual string Data { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> _additionalData;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // data is not deserialized to any property and so it is added to the extension data dictionary
            Data = (string)_additionalData["data"].ToString();
        }
        public bool ShouldSerializeDateCreatedUT()
        {
            return false;
        }
    }
}
