// using System;
// using System.Security.Cryptography;
// using System.Threading.Tasks;
// using Csa.Domain.Auth;
// using Ezley.Domain;
// using Ezley.Domain.CRM;
// using Ezley.EventStore;
// using Ezley.ValueObjects;
// using Xunit;
//
// namespace Ezley.Testing
// {
//     public class RunProjectionEngine
//     {
//         private TestConfig _testConfig = new TestConfig();
//
//          
//         [Fact]
//         public void RegisterProjectionsAsync()
//         {
//             var eventTypeResolver = new EventTypeResolver();
//             var viewRepo = new CosmosDBViewRepository(
//                 _testConfig.ViewsEndpointUri, 
//                 _testConfig.ViewsAuthKey,
//                 _testConfig.ViewsDatabase,
//                 _testConfig.ViewsContainer);
//             
//             var processorName = "UnitTestsProcessor";
//             var projectionEngine = new CosmosDBProjectionEngine(eventTypeResolver, viewRepo, processorName,
//                 _testConfig.EventsEndpointUri, _testConfig.EventsAuthKey, _testConfig.EventsDatabase,
//                 _testConfig.LeasesEndpointUri, _testConfig.LeasesAuthKey, _testConfig.LeasesDatabase,
//                 _testConfig.EventContainer, _testConfig.LeasesContainer, _testConfig.StartTimeEpoch);
//             
//             await projectionEngine.StartAsync("UnitTests");
//               projectionEngine.RegisterProjection(new UserProjection());
//             // projectionEngine.RegisterProjection(new CustomerProjection());
//              
//             await projectionEngine.StartAsync("UnitTests");
//             await Task.Delay(-1);
//         }
//         
//     }
// }