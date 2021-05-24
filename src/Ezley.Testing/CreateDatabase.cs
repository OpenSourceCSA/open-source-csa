using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Xunit;

namespace Ezley.Testing
{
    public class CreateDatabase
    {
        private readonly string _endpointUrl;
        private readonly string _authorizationKey;
        private const string _databaseId = "csadb";
        private const string _sp = "spAppendToStream";
        
        // core containers
        private const string _events = "events";
        private const string _leases = "leases";
        private const string _views = "views";
        private const string _snapshots = "snapshots";
        
        // work flow engine containers
        private const string _wfeleases = "wfeleases";
        private const string _wfeviews = "wfeviews";
        
        public CreateDatabase()
        {
            
           _endpointUrl = Environment.GetEnvironmentVariable("COSMOSDB_EVENT_SOURCING_URL");// "https://cosmoseventsourcing.documents.azure.com:443/";
           _authorizationKey = Environment.GetEnvironmentVariable("COSMOSDB_EVENT_SOURCING_KEY");
            
        }
        [Fact]
        public async Task SC00_MigrateDB()
        {
            CosmosClient client = new CosmosClient(_endpointUrl, _authorizationKey);
            var body = File.ReadAllText($"js/{_sp}.js");
            await client.CreateDatabaseIfNotExistsAsync(_databaseId, ThroughputProperties.CreateManualThroughput(400));
            Database database = client.GetDatabase(_databaseId);
            
            await database.DefineContainer(_events, "/stream/id").CreateIfNotExistsAsync();
            await database.DefineContainer(_leases, "/id").CreateIfNotExistsAsync();
            await database.DefineContainer(_views, "/id").CreateIfNotExistsAsync();
            await database.DefineContainer(_snapshots, "/id").CreateIfNotExistsAsync();
            await database.DefineContainer(_wfeleases, "/id").CreateIfNotExistsAsync();
            await database.DefineContainer(_wfeviews, "/id")
                .WithIndexingPolicy()
                .WithIncludedPaths()
                .Path( "/*")
                .Attach()
                .WithExcludedPaths()
                .Path("/payload/*")
                .Attach()
                .Attach()
                .CreateIfNotExistsAsync();
            
            StoredProcedureProperties storedProcedureProperties = new StoredProcedureProperties
            {
                Id = _sp,
                Body = File.ReadAllText($"js/{_sp}.js"),
            };
            
            Container eventsContainer = database.GetContainer(_events);
            try
            {
                await eventsContainer.Scripts.DeleteStoredProcedureAsync(_sp);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Stored procedure didn't exist yet.
            } 
            await eventsContainer.Scripts.CreateStoredProcedureAsync(storedProcedureProperties);
        }
    }
}