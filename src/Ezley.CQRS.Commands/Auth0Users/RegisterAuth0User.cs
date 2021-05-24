using System;
using System.Threading;
using System.Threading.Tasks;
using Csa.Domain.Auth;
using ES.Domain;
using Ezley.EventStore;
using MediatR;

namespace Ezley.Commands.Auth0Users
{
    public class RegisterAuth0User: IRequest
    {
        public EventUserInfo EventUserInfo { get; private set; }
        public string Id { get; private set; }
        public Guid UserId { get; private set; }

        public RegisterAuth0User(EventUserInfo eventUserInfo, string id, Guid userId)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            UserId = userId;
        }
    }
    
    public class RegisterAuth0UserHandler : IRequestHandler<RegisterAuth0User>
    {
        private IRepository _repository;

        public RegisterAuth0UserHandler(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            RegisterAuth0User command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = new Auth0User(command.Id, command.UserId);
            
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}