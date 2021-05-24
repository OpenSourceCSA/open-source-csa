using System;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using MediatR;

namespace Ezley.Commands
{
    public class ChangeUserAuth0Id:IRequest
    {
        public EventUserInfo EventUserInfo { get; private set; }
        public Guid Id { get; private set; }
        public string Auth0Id { get; private set; }

        public ChangeUserAuth0Id(EventUserInfo eventUserInfo,
            Guid id, string auth0Id)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Auth0Id = auth0Id;
        }
        
    }
    public class ChangeUserAuth0IdHandler : IRequestHandler<ChangeUserAuth0Id>
    {
        private IRepository _repository;

        public ChangeUserAuth0IdHandler(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            ChangeUserAuth0Id command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            user.ChangeAuth0Id(command.Auth0Id);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}