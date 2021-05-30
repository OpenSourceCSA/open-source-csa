using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        
        public readonly string EndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string Database = "esdemo";
        public readonly string AuthKey = "your key ==";
        
        public readonly string EventsContainer = "events";
        public readonly string KeysContainer = "keys";
        public readonly string LeasesContainer = "leases";
        public readonly string ViewsContainer = "orderviews";
        public readonly string WFELeases = "wfeleases";
        public readonly string WFEViews = "wfeviews";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}