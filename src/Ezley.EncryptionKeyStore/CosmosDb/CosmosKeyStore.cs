using System;
using System.Net;
using System.Threading.Tasks;
using Ezley.EncyrptionKeyStore;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;

namespace Ezley.SnapshotStore
{
    public class CosmosDBKeyStore : IKeyStore
    {
        private readonly CosmosClient _client;
        private readonly string _databaseId;
        private readonly string _containerId;

        public CosmosDBKeyStore(string endpointUrl, string authorizationKey,
            string databaseId, string containerId)
        {
            _client = new CosmosClient(endpointUrl, authorizationKey);
            _databaseId = databaseId;
            _containerId = containerId;
        }

        public async Task<Key> LoadKeyAsync(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            try
            {
                var response = await container.ReadItemAsync<Key>(streamId, partitionKey);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Resource;
                }
            }
            catch (Microsoft.Azure.Cosmos.CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task SaveKeyAsync(string streamId, object key)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            await container.UpsertItemAsync(new Key
            {
                StreamId = streamId,
                KeyData = JObject.FromObject(key)
            }, partitionKey);
        }
    }
}