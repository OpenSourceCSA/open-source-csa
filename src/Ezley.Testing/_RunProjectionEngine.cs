using System.Threading.Tasks;
using Ezley.Events;
using Ezley.Projections;
using Ezley.ProjectionStore;
using Xunit;

namespace Ezley.Testing
{
    public class RunProjectionEngine
    {
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task RegisterProjectionsAsync()
        {
            var eventTypeResolver = new EventTypeResolver();

            var viewRepo = RepositoryHelper.GetViewRepository();
            var processorName = "UnitTestsProcessor";
            var projectionEngine = new CosmosDBProjectionEngine(eventTypeResolver, viewRepo, processorName,
                _testConfig.EndpointUri, _testConfig.AuthKey, _testConfig.Database,
                _testConfig.EndpointUri, _testConfig.AuthKey, _testConfig.Database,
                _testConfig.EventsContainer, _testConfig.LeasesContainer, _testConfig.StartTimeEpoch);

            var repo = RepositoryHelper.GetRepository();

            await projectionEngine.StartAsync("UnitTests");
            projectionEngine.RegisterProjection(new UserProjection(repo));
            
            await Task.Delay(-1);
        }
    }
}