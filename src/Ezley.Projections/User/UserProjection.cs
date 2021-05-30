using System;
using ES.Domain;
using Ezley.Events;
using Ezley.ProjectionStore;
using Ezley.ValueObjects;

namespace Ezley.Projections
{
    public class UserView
    {
        public Guid Id { get; set; }
        public PersonName PersonName { get; set; }
        public DisplayName DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }
        public bool Active { get; set; }
        public string Auth0Id { get; set; }
    }

    public class UserProjection : Projection<UserView>
    {
        public readonly IRepository _respository;

        public UserProjection(IRepository repository)
        {
            _respository = repository;
            RegisterHandler<UserRegistered>(OnRegistered);
            RegisterHandler<UserDisplayNameChanged>(OnDisplayNameChanged);
            RegisterHandler<UserPersonNameChanged>(OnPersonNameChanged);
            RegisterHandler<UserAddressChanged>(OnUserAddressChanged);
            RegisterHandler<UserEmailChanged>(OnUserEmailChanged);
            RegisterHandler<UserPhoneChanged>(OnUserPhoneChanged);
            RegisterHandler<UserAuth0IdChanged>(OnUserAuth0IdChanged);
            RegisterHandler<UserDeactivated>(OnUserDeactivated);
            RegisterHandler<UserActivated>(OnUserActivated);
        }

        private byte[] GetKey(string id)
        {
            return _respository.LoadKeyAsync(id).GetAwaiter().GetResult();
        }

        private void OnRegistered(UserRegistered e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.Id = e.Id;
            view.DisplayName = DisplayName.Create(e.EncDisplayName, key);
            view.Active = e.Active;
            view.Address = Address.Create(e.EncAddress, key);
            view.Email = Email.Create(e.EncEmail, key);
            view.Phone = Phone.Create(e.EncPhone, key);
            view.PersonName = PersonName.Create(e.EncPersonName, key);
            view.Auth0Id = e.Auth0Id;
        }

        private void OnDisplayNameChanged(UserDisplayNameChanged e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.DisplayName = DisplayName.Create(e.EncDisplayName, key);
        }

        private void OnPersonNameChanged(UserPersonNameChanged e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.PersonName = PersonName.Create(e.EncPersonName, key);
        }

        private void OnUserAddressChanged(UserAddressChanged e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.Address = Address.Create(e.EncAddress, key);
        }

        private void OnUserEmailChanged(UserEmailChanged e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.Email = Email.Create(e.EncEmail, key);
        }

        private void OnUserPhoneChanged(UserPhoneChanged e, UserView view)
        {
            var key = GetKey(e.Id.ToString());

            view.Phone = Phone.Create(e.EncPhone, key);
        }

        private void OnUserAuth0IdChanged(UserAuth0IdChanged e, UserView view)
        {
            view.Auth0Id = e.Auth0Id;
        }

        private void OnUserDeactivated(UserDeactivated e, UserView view)
        {
            view.Active = false;
        }

        private void OnUserActivated(UserActivated e, UserView view)
        {
            view.Active = true;
        }
    }
}