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
    public class AddConsumer:IRequest
    {
        public EventUserInfo EventUserInfo { get;  }
       public Guid Id { get; }
        public string Auth0Id { get;  }
        public Guid ServiceProviderId { get;  }
        public Guid ServiceSubscriberId { get;  }
        public PersonName PersonName { get;  }
        public string DisplayName { get;  }
        public Address Address { get;  }
        public Phone Phone { get;  }
        public Email Email { get;  }
        public bool Active { get;  }

        public AddConsumer(EventUserInfo eventUserInfo,
            Guid id, 
            string auth0Id, Guid serviceProviderId, PersonName personName, string displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Auth0Id = auth0Id;
            ServiceProviderId = serviceProviderId;
            PersonName = personName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
        public AddConsumer(EventUserInfo eventUserInfo,
            Guid id, 
            string auth0Id, Guid serviceProviderId, Guid serviceSubscriberId, PersonName personName, string displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Auth0Id = auth0Id;
            ServiceProviderId = serviceProviderId;
            ServiceSubscriberId = serviceSubscriberId;
            PersonName = personName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }
    
    public class AddConsumerHandler : IRequestHandler<AddConsumer>
    {
        private IRepository _repository;
        public AddConsumerHandler(IRepository repository)
        {
            _repository = repository;
        }
        

        public async Task<Unit> Handle(
            AddConsumer command,
            CancellationToken cancellationToken)
        {
         
            // Domain BRs
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");
            if (command.ServiceProviderId == Guid.Empty)
                throw new ApplicationException("ServiceProviderId is required.");

            // TODO:
            // var user = new Consumer(command.Id, command.ServiceProviderId, 
            //     command.PersonName, command.DisplayName,
            //     command.Address, command.Phone, command.Email, true);
            //
            // await _repository.Save(command.EventUserInfo, user);
             
            return Unit.Value;
        }
#pragma warning disable 1998
  
    }
}