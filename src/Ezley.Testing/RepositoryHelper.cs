using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;

namespace Ezley.Testing
{
    public static class RepositoryHelper
    {
        private static readonly string _endpointUrl;
        private static readonly string _authorizationKey;
        private static readonly string _databaseId = "csadb";
        private static readonly string _sp = "spAppendToStream";
        
        // core containers
        private static readonly string _events = "events";
        private static readonly string _leases = "leases";
        private static readonly string _views = "views";
        private static readonly string _snapshots = "snapshots";
        
        // work flow engine containers
        private static readonly string _wfeleases = "wfeleases";
        private static readonly string _wfeviews = "wfeviews";

        private static readonly bool _useInMemDb = false;
        
        static RepositoryHelper()
        {
            _endpointUrl = Environment.GetEnvironmentVariable("COSMOSDB_EVENT_SOURCING_URL");
            _authorizationKey = Environment.GetEnvironmentVariable("COSMOSDB_EVENT_SOURCING_KEY");
        }
        public static Repository GetRepository()
        {
            var eventTypeResolver = new EventTypeResolver();
            if (_useInMemDb)
            {
                var eventStore = new InMemoryEventStore(
                    eventTypeResolver,
                    new Dictionary<string, List<string>>());
                return new Repository(eventStore, null);
            }
            else
            {
                var eventStore = new CosmosDBEventStore(
                    eventTypeResolver,
                    _endpointUrl,
                    _authorizationKey,
                    _databaseId,
                    _events);
                
                return new Repository(eventStore, null);
            }
        }
       
        public static CosmosDBViewRepository GetViewRepository()
        {
            return new CosmosDBViewRepository(_endpointUrl,
                _authorizationKey,
               _databaseId,
                _views);
        }
    }
}