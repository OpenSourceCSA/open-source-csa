using System;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;
using MediatR;

namespace Ezley.Commands
{
    public class ChangeUserPersonName : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public PersonName PersonName { get; }

        public ChangeUserPersonName(
            EventUserInfo eventUserInfo,
            Guid id, PersonName personName)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            PersonName = personName;
        }
    }

    public class ChangeUserPersonNameHandler : IRequestHandler<ChangeUserPersonName>
    {
        private IRepository _repository;

        public ChangeUserPersonNameHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            ChangeUserPersonName command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            var keyInfo = await _repository.LoadKeyInfoAsync(user.Id);
            
            user.ChangePersonName(keyInfo, command.PersonName);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}