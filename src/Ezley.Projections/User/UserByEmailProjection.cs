using System;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;
using Ezley.ValueObjects;

namespace Ezley.Projections
{
    public class UserByEmailView
    {
        public Guid Id { get; set; }
    }

    public class UserByEmailProjection : Projection<UserByEmailView>
    {
        public readonly IRepository _respository;

        public UserByEmailProjection(IRepository repository)
        {
            _respository = repository;
            RegisterHandler<UserRegistered>(OnRegistered);
            RegisterHandler<UserEmailChanged>(OnUserEmailChanged);
        }

        public override string[] GetViewNames(string streamId, IEvent @event)
        {
            string emailAddress = GetEmailAddressFromEvent(@event);
            return new[] {emailAddress};
        }

        private string GetEmailAddressFromEvent(dynamic @event)
        {
            return GetEmailAddress(@event);
        }

        private string GetEmailAddress(UserRegistered @event)
        {
            var key = GetKey(@event.Id);
            var emailAddress = new Email(@event.EncEmail, key);
            return emailAddress.Address;
        }

        private string GetEmailAddress(UserEmailChanged @event)
        {
            var key = GetKey(@event.Id);
            var emailAddress = new Email(@event.EncEmail, key);
            return emailAddress.Address;
        }

        private byte[] GetKey(Guid id)
        {
            return _respository.LoadKeyAsync(id.ToString());
        }

        #region EventHandlers

        private void OnRegistered(UserRegistered e, UserByEmailView view)
        {
            view.Id = e.Id;
        }

        private void OnUserEmailChanged(UserEmailChanged e, UserByEmailView view)
        {
            view.Id = e.Id;
        }

        #endregion
    }
}