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
    public class ChangeUserPhone : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public Phone Phone { get; }

        public ChangeUserPhone(
            EventUserInfo eventUserInfo,
            Guid id, Phone phone)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Phone = phone;
        }
    }

    public class ChangeUserPhoneHandler : IRequestHandler<ChangeUserPhone>
    {
        private IRepository _repository;

        public ChangeUserPhoneHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            ChangeUserPhone command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            var keyInfo = await _repository.LoadKeyInfoAsync(command.Id.ToString());
            user.ChangePhone(keyInfo, command.Phone);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}