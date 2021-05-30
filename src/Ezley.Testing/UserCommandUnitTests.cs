using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Commands;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Moq;
using Xunit;

namespace Ezley.Testing
{
    public class UserCommandsUnitTests
    {
        [Fact]
        public async Task AddUser()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);

            var testUser = new TestUser(id);
            var user = CreateUserHelper(testUser, ki);

            var eventUserInfo = GetAnonymousUser();

            var repo = new Mock<IRepository>();
            repo.Setup(x => x
                    .Load<User>(It.IsAny<Guid>()))
                .ReturnsAsync(user);
            repo.Setup(x => x
                    .Save(It.IsAny<EventUserInfo>(), It.IsAny<User>()))
                .ReturnsAsync(true);

            var token = new CancellationToken();

            var command = new RegisterUser(eventUserInfo,
                testUser.id,
                testUser.auth0Id,
                testUser.personName,
                testUser.displayName,
                testUser.address,
                testUser.phone,
                testUser.email,
                testUser.active);
            var handler = new RegisterUserHandler(repo.Object);

            await handler.Handle(command, token);
        }

        [Fact]
        public async Task AddUser_ShouldError()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);

            var testUser = new TestUser(id);
            var user = CreateUserHelper(testUser, ki);

            var eventUserInfo = GetApiUser();

            var repo = new Mock<IRepository>();
            repo.Setup(x => x
                    .Load<User>(It.IsAny<Guid>()))
                .ReturnsAsync(user);
            repo.Setup(x => x
                    .Save(It.IsAny<EventUserInfo>(), It.IsAny<User>()))
                .ReturnsAsync(true);

            var token = new CancellationToken();

            var command = new RegisterUser(eventUserInfo,
                testUser.id,
                testUser.auth0Id,
                testUser.personName,
                testUser.displayName,
                testUser.address,
                testUser.phone,
                testUser.email,
                testUser.active);
            var handler = new RegisterUserHandler(repo.Object);
            
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, token));
        }

        [Fact]
        public async Task ChangeDisplayName()
        {
            Aes aes = Aes.Create();
            Guid id = Guid.NewGuid();
            AesKeyInfo ki = new AesKeyInfo(id.ToString(), aes.Key, aes.IV);

            var testUser = new TestUser(id);
            var user = CreateUserHelper(testUser, ki);

            var eventUserInfo = GetTokenUser();

            var repo = new Mock<IRepository>();
            repo.Setup(x => x
                    .Load<User>(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            repo.Setup(x => x.LoadKeyInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(ki);
            repo.Setup(x => x
                    .Save(It.IsAny<EventUserInfo>(), It.IsAny<User>()))
                .ReturnsAsync(true);

            var token = new CancellationToken();

            var command = new ChangeUserDisplayName(
                eventUserInfo,
                testUser.id,
                testUser.displayName
            );
            var handler = new ChangeUserDisplayNameHandler(repo.Object);
            await handler.Handle(command, token);
            
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

    public class TestUser
    {
        public TestUser(Guid id)
        {
            this.id = id;
        }

        public Guid id;
        public string auth0Id = "auth|someuser";
        public PersonName personName = new PersonName("John", "Doe");
        public DisplayName displayName = new DisplayName("JohnnyD");

        public Address address = new Address("123 Main Street",
            "Suite 105",
            "Door B",
            "Pleasant Garden",
            "27313",
            "NC",
            "USA");

        public Phone phone = new Phone("1", "6761261");
        public Email email = new Email("jdoe@example.net");
        public bool active = true;
    }
}