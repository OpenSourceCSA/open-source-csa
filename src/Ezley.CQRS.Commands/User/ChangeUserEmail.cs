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
    public class ChangeUserEmail:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
        public Guid Id { get;  }
        public Email Email { get;  }

        public ChangeUserEmail(EventUserInfo eventUserInfo,
            Guid id, Email email)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Email = email;
        }
    }
    
    public class ChangeUserEmailHandler : IRequestHandler<ChangeUserEmail>
    {
        private IRepository _repository;

        public ChangeUserEmailHandler(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Unit> Handle(
            ChangeUserEmail command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var user = await _repository.Load<User>(command.Id);
            var keyInfo = await _repository.LoadKeyInfoAsync(user.Id);
            
            user.ChangeEmail(keyInfo, command.Email);
            await _repository.Save(command.EventUserInfo, user);
            return Unit.Value;
        }
    }
}