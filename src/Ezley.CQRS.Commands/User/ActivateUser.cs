using System;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using MediatR;

namespace Ezley.Commands
{
    public class ActivateUser : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }

        public ActivateUser(
            EventUserInfo eventUserInfo,
            Guid id, Address address)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
        }
    }

    public class ActivateUserHandler : IRequestHandler<ActivateUser>
    {
        private IRepository _repository;

        public ActivateUserHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            ActivateUser command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            user.Activate();
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}