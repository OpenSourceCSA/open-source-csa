using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Commands;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.Projections;
using Ezley.ValueObjects;
using Moq;
using Xunit;

namespace Ezley.Testing
{
    public class UserProjectionTests
    {
        private int _delayMs = 5000;

        [Fact]
        public async Task AddUser()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);

            var user = new TestUser(id);

            var eventUserInfo = GetAnonymousUser();

            var repo = RepositoryHelper.GetRepository();

            var userAgg = new User(ki,
                user.id,
                user.auth0Id,
                user.personName,
                user.displayName,
                user.address,
                user.phone,
                user.email,
                user.active);
            bool savedKey = await repo.SaveKeyInfo(ki);
            bool saved = await repo.Save(eventUserInfo, userAgg);

            await Task.Delay(_delayMs);

            var viewRepo = RepositoryHelper.GetViewRepository();
            var viewName = $"{nameof(UserView)}:{userAgg.Id}";
            var projection = await viewRepo.LoadTypedViewAsync<UserView>(viewName);
            Assert.Equal(user.id, projection.Id);
            Assert.Equal(user.active, projection.Active);
            Assert.Equal(user.address, projection.Address);
            Assert.Equal(user.email, projection.Email);
            Assert.Equal(user.phone, projection.Phone);
            Assert.Equal(user.auth0Id, projection.Auth0Id);
            Assert.Equal(user.displayName, projection.DisplayName);
            Assert.Equal(user.personName, projection.PersonName);
        }
     

       
        

        private User CreateUserHelper(TestUser testUser, AesKeyInfo keyInfo)
        {
            var user = new User(keyInfo, 
                testUser.id,
                testUser.auth0Id,
                testUser.personName,
                testUser.displayName,
                testUser.address,
                testUser.phone,
                testUser.email,
                testUser.active);

            user.ChangeAuth0Id(testUser.auth0Id);
            return user;
        }

        private EventUserInfo GetAnonymousUser()
        {
            var eventUserInfo = new EventUserInfo("", false);
            return eventUserInfo;
        }
        private EventUserInfo GetTokenUser()
        {
            var eventUserInfo = new EventUserInfo("auth0|tokenuser1234", false);
            return eventUserInfo;
        }
        private EventUserInfo GetApiUser()
        {
            var eventUserInfo = new EventUserInfo("", true);
            return eventUserInfo;
        }
    }
}