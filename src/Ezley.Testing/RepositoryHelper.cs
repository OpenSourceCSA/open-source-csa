using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;
using Ezley.SnapshotStore;

namespace Ezley.Testing
{
    public static class RepositoryHelper
    {
        private static readonly string _endpointUrl;
        private static readonly string _authorizationKey;
        private static readonly string _databaseId;
        //private static readonly string _sp = "spAppendToStream";
        
        // core containers
        private static readonly string _events;
        private static readonly string _keys;
        private static readonly string _leases;
        private static readonly string _views;
        private static readonly string _snapshots;
        
        // work flow engine containers
        private static readonly string _wfeleases;
        private static readonly string _wfeviews ;

        private static readonly bool _useInMemDb = false;
        
        static RepositoryHelper()
        {
             var testConfig = new TestConfig();
            _endpointUrl = testConfig.EndpointUri;
            _authorizationKey = testConfig.AuthKey; 
            _databaseId = testConfig.Database;
            _events = testConfig.EventsContainer;
            _keys = testConfig.KeysContainer;
            _leases = testConfig.LeasesContainer;
            _views = testConfig.ViewsContainer;
            _wfeleases = testConfig.WFELeases;
            _wfeviews = testConfig.WFEViews;
            _snapshots = testConfig.SnapshotsContainer;

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
                var keyStore = new CosmosDBKeyStore(_endpointUrl, _authorizationKey, _databaseId, _keys);
                var eventStore = new CosmosDBEventStore(
                    eventTypeResolver,
                    _endpointUrl,
                    _authorizationKey,
                    _databaseId,
                    _events);
                
                return new Repository(eventStore, keyStore);
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