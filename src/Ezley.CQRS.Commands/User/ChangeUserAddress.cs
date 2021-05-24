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
    public class ChangeUserAddress:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public Address Address { get;  }

        public ChangeUserAddress(EventUserInfo eventUserInfo,
            Guid id, Address address)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Address = address;
        }
    }
    
    public class ChangeUserAddressHandler : IRequestHandler<ChangeUserAddress>
    {
        private IRepository _repository;

        public ChangeUserAddressHandler(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            ChangeUserAddress command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            var keyInfo = await _repository.LoadKeyInfoAsync(user.Id);
            user.ChangeAddress(keyInfo, command.Address);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}