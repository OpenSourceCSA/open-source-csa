using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Csa.Domain.Auth;
using ES.Domain;
using Ezley.Commands;
using Ezley.Commands.Auth0Users;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Moq;
using Xunit;

namespace Ezley.Testing
{
    public class Auth0UserCommandsUnitTests
    {
        [Fact]
        public async Task AddUser()
        {
            Guid id = Guid.NewGuid();

            var auth0User = CreateAuth0UserHelper(id);

            var eventUserInfo = GetAnonymousUser();

            var repo = new Mock<IRepository>();
            // repo.Setup(x => x
            //         .Load<Auth0User>(It.IsAny<Guid>()))
            //     .ReturnsAsync(auth0User);
            repo.Setup(x => x
                    .Save(It.IsAny<EventUserInfo>(), It.IsAny<Auth0User>()))
                .ReturnsAsync(true);

            var token = new CancellationToken();

            var command = new RegisterAuth0User(eventUserInfo,
                auth0User.Id, auth0User.UserId);
            
            var handler = new RegisterAuth0UserHandler(repo.Object);

            var unit = await handler.Handle(command, token);
        }

      
        private Auth0User CreateAuth0UserHelper(Guid id)
        {
            return new Auth0User(id.ToString(), Guid.NewGuid());
        }

        private EventUserInfo GetAnonymousUser()
        {
            var eventUserInfo = new EventUserInfo("", false);
            return eventUserInfo;
        }

        private EventUserInfo GetApiUser()
        {
            var eventUserInfo = new EventUserInfo("", true);
            return eventUserInfo;
        }
    }
}