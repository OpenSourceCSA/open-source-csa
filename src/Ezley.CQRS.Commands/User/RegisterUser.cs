using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ES.Domain;
using Ezley.Domain;
using Ezley.Domain.CRM;
using Ezley.EventStore;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;
using MediatR;
using Encoder = ApplicationServices.Encoder;

namespace Ezley.Commands
{
    public class RegisterUser : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public string Auth0Id { get; }
        public PersonName PersonName { get; }
        public DisplayName DisplayName { get; }
        public Address Address { get; }
        public Phone Phone { get; }
        public Email Email { get; }
        public bool Active { get; }


        public RegisterUser(
            EventUserInfo eventUserInfo,
            Guid id,
            string auth0Id, PersonName personName, DisplayName displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Auth0Id = auth0Id;
            PersonName = personName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }

    public class RegisterUserHandler : IRequestHandler<RegisterUser>
    {
        private IRepository _repository;

        public RegisterUserHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            RegisterUser command,
            CancellationToken cancellationToken)
        {
            command.EventUserInfo.VerifyIsAnonymous_ThrowsException();
            
            Aes myAes = Aes.Create();
            AesKeyInfo aesInfo = new AesKeyInfo(
                command.Id.ToString(),
                myAes.Key,
                myAes.IV);
            await _repository.SaveKeyInfo(aesInfo);

            var user = new User(
                aesInfo,
                command.Id,
                command.Auth0Id,
                command.PersonName,
                command.DisplayName,
                command.Address,
                command.Phone,
                command.Email,
                command.Active);

            await _repository.Save(command.EventUserInfo, user);

            return Unit.Value;
        }
    }
}