using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ezley.EncyrptionKeyStore
{
    public class Key
    {
        [JsonProperty("id")]
        public string StreamId { get; set; }

        [JsonProperty("keyData")]
        public JObject KeyData { get; set; }
    }
}