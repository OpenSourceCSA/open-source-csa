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
    public class DeactivateUser : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        
        public string Reason { get; }

        public DeactivateUser(
            EventUserInfo eventUserInfo,
            Guid id, string reason)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Reason = reason;
        }
    }

    public class DeactivateUserHandler : IRequestHandler<DeactivateUser>
    {
        private IRepository _repository;

        public DeactivateUserHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeactivateUser command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id.ToString());
            user.Deactivate();
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}