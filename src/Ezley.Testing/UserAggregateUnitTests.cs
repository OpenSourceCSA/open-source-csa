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
            Assert.Equal(user.EncPersonName,agg.EncPersonName);
            Assert.Equal(user.EncDisplayName, agg.EncDisplayName);
            Assert.Equal(user.EncAddress, agg.EncAddress);
            Assert.Equal(user.EncPhone, agg.EncPhone);
            Assert.Equal(user.EncEmail, agg.EncEmail);
            Assert.Equal(user.Active, agg.Active);
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
            await repo.Save(eventUserInfo, agg1);

            var agg = await repo.Load<User>(id);
          
            Assert.Equal(id.ToString(), agg.Id);
            Assert.Equal(user.EncPersonName,agg.EncPersonName);
            Assert.Equal(user.EncDisplayName, agg.EncDisplayName);
            Assert.Equal(user.EncAddress, agg.EncAddress);
            Assert.Equal(user.EncPhone, agg.EncPhone);
            Assert.Equal(user.EncEmail, agg.EncEmail);
            Assert.Equal(user.Active, agg.Active);
        }

        private User CreateUserHelper(Guid userId, AesKeyInfo keyInfo)
        {
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
             
            var user = new User(keyInfo, userId, personName, displayName, address, phone, email, active);
            return user;
        }

        private EventUserInfo GetAnonymousUser()
        {
            var eventUserInfo = new EventUserInfo("anonymous", false);
            return eventUserInfo;
        }
    }
}