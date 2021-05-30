using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Csa.Domain.Auth;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Xunit;

namespace Ezley.Testing
{
    public class Auth0UserAggregateUnitTests
    {
        [Fact]
        public async Task RegisterAuth0User()
        {
            Guid id = Guid.NewGuid();
            var auth0User = CreateAuth0User(id);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, auth0User);

            var agg = await repo.Load<Auth0User>(id);

            Assert.Equal(id.ToString(), agg.Id);
            Assert.Equal(auth0User.UserId, agg.UserId);
        }

        

        private Auth0User CreateAuth0User(Guid id)
        {
            var userId = Guid.NewGuid();

            var auth0User = new Auth0User(id.ToString(), userId);
            return auth0User;
        }

        private EventUserInfo GetAnonymousUser()
        {
            var eventUserInfo = new EventUserInfo("anonymous", false);
            return eventUserInfo;
        }
    }
}