using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Xunit;

namespace Ezley.Testing
{
    public class UserAggregateUnitTests
    {
        [Fact]
        public async Task AddUser()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg = await repo.Load<User>(id);
          
            Assert.Equal(id.ToString(), agg.Id);
            Assert.Equal(user.Auth0Id, agg.Auth0Id);
            Assert.Equal(user.EncPersonName,agg.EncPersonName);
            Assert.Equal(user.EncDisplayName, agg.EncDisplayName);
            Assert.Equal(user.EncAddress, agg.EncAddress);
            Assert.Equal(user.EncPhone, agg.EncPhone);
            Assert.Equal(user.EncEmail, agg.EncEmail);
            Assert.Equal(user.Active, agg.Active);
        }
        
        [Fact]
        public async Task ChangeUserAddress()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var newAddress = new Address("1Line", "2Line", "3Line", "newCity", "32323",
                "WV", "newUs");
            agg1.ChangeAddress(ki, newAddress);
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
           
            Assert.Equal(agg1.EncAddress, agg.EncAddress);
           
        }
        
        [Fact]
        public async Task ChangeUserAuth0Id()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var auth0Id = "newAuth0Id";
            agg1.ChangeAuth0Id(auth0Id);
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
            
            Assert.Equal(agg1.Auth0Id, agg.Auth0Id);
        }

        [Fact]
        public async Task ChangeDisplayName()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var newDisplayName = new DisplayName("Fred");
            agg1.ChangeDisplayName(ki, newDisplayName);
           saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
          
            Assert.Equal(agg1.EncDisplayName, agg.EncDisplayName);
        }
        
        [Fact]
        public async Task ChangeUserEmail()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var newEmail = new Email("mynewemail@aol.com");
            agg1.ChangeEmail(ki, newEmail);
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
            
            Assert.Equal(agg1.EncEmail, agg.EncEmail);
            
        }

        [Fact]
        public async Task ChangeUserPersonName()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var newPersonName = new PersonName("newFirstName", "newLastName");
            agg1.ChangePersonName(ki, newPersonName);
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
            
            Assert.Equal(agg1.EncPersonName, agg.EncPersonName);
            
        }
        [Fact]
        public async Task ChangeUserPhone()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            var newPhone = new Phone("3", "555-765-0099");
            agg1.ChangePhone(ki, newPhone);
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
            
            Assert.Equal(agg1.EncPhone, agg.EncPhone);
        }
        
        [Fact]
        public async Task ChangeUserDeactivateUser()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);
            var user = CreateUserHelper(id, ki);

            var eventUserInfo = GetAnonymousUser();
            var repo = RepositoryHelper.GetRepository();
            var saved = await repo.Save(eventUserInfo, user);

            var agg1 = await repo.Load<User>(id);
            agg1.Deactivate();
            saved =  await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
            
            Assert.False(agg.Active);
        }
        private User CreateUserHelper(Guid userId, AesKeyInfo keyInfo)
        {
            var auth0Id = $"auth0|{userId}";
            var personName = new PersonName("John", "Doe");
            var displayName = new DisplayName("JohnnyD");
            var address = new Address("123 Main Street",
                "Suite 105",
                "Door B",
                "Pleasant Garden",
                "27313",
                "NC",
                "USA");
            var phone = new Phone("1", "6761261");
            var email = new Email("jdoe@example.net");
            var active = true;
             
            var user = new User(keyInfo, userId, auth0Id, personName, displayName, address, phone, email, active);
            return user;
        }

        private EventUserInfo GetAnonymousUser()
        {
            var eventUserInfo = new EventUserInfo("anonymous", false);
            return eventUserInfo;
        }
    }
}