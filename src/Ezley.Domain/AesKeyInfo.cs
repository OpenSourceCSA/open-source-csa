using System;
using Newtonsoft.Json;

namespace Ezley.Domain
{
    public class AesKeyInfo
    {
        /// <summary>
        /// This key should match the Aggregate that it goes with.
        /// </summary>
        [JsonProperty]
        public string Id { get;  }
        [JsonProperty]
        public byte[] Key { get;  }
        [JsonProperty]
        public byte[] IV { get;  }
        [JsonProperty]
        public bool Destroyed { get; private set; }
        [JsonProperty]
        public DateTime? DestroyedDateTimeUtc { get; private set;}
        [JsonProperty]
        public string DestroyedReasonComment { get; private set; }
        
        public AesKeyInfo(string id, byte[] key, byte[] iv)
        {
            Id = id;
            Key = key;
            IV = iv;
            Destroyed = false;
            DestroyedReasonComment = "";
        }

        public void DestroyKey(string reasonComment)
        {
            Destroyed = true;
            DestroyedDateTimeUtc = DateTime.UtcNow;
            DestroyedReasonComment = reasonComment;
        }
    
    }
}