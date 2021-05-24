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
    public class ChangeUserDisplayName:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public DisplayName DisplayName { get;  }

        public ChangeUserDisplayName(EventUserInfo eventUserInfo,
            Guid id, DisplayName displayName)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            DisplayName = displayName;
        }
    }
    
    public class ChangeUserDisplayNameHandler : IRequestHandler<ChangeUserDisplayName>
    {
        private IRepository _repository;

        public ChangeUserDisplayNameHandler(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            ChangeUserDisplayName command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            var keyInfo = await _repository.LoadKeyInfoAsync(user.Id);
            user.ChangeDisplayName(keyInfo, command.DisplayName);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}