using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Domain.CRM
{
    public class User: AggregateBase
    {
        public EncryptedPersonName EncPersonName { get; private set; }
        public EncryptedDisplayName EncDisplayName { get; private set; }
        public EncryptedAddress EncAddress { get; private set; }
        public EncryptedPhone EncPhone { get; private set; }
        public EncryptedEmail EncEmail { get; private set; }
        public bool Active { get; private set; }
        public string Auth0Id { get; private set; }

        public User(IEnumerable<IEvent> events): base(events)
        {
        }
        public User(AesKeyInfo keyInfo, Guid id, string auth0Id, PersonName personName, DisplayName displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            Apply( new UserRegistered(
                id, 
                auth0Id,
                new EncryptedPersonName(personName, keyInfo.Key, keyInfo.IV),
                new EncryptedDisplayName (displayName, keyInfo.Key, keyInfo.IV),
                new EncryptedAddress(address, keyInfo.Key, keyInfo.IV),
                new EncryptedPhone(phone, keyInfo.Key, keyInfo.IV),
                new EncryptedEmail(email, keyInfo.Key, keyInfo.IV), 
                active));
        }
       
        public void ChangePersonName(AesKeyInfo keyInfo, PersonName personName)
        {
            EncryptedPersonName encPersonName = new EncryptedPersonName(personName, keyInfo.Key, keyInfo.IV);
            if(this.EncPersonName != encPersonName)
                this.Apply(new UserPersonNameChanged(Guid.Parse(Id), encPersonName));
        }
        public void ChangeDisplayName(AesKeyInfo keyInfo, DisplayName displayName)
        {
            EncryptedDisplayName encryptedDisplayName = new EncryptedDisplayName(displayName, keyInfo.Key, keyInfo.IV);
            if(EncDisplayName != encryptedDisplayName)
                this.Apply(new UserDisplayNameChanged(Guid.Parse(Id), encryptedDisplayName));
        }
        public void ChangeAddress(AesKeyInfo keyInfo, Address address)
        {
            EncryptedAddress encryptedAddress = new EncryptedAddress(address, keyInfo.Key, keyInfo.IV);
            if(EncAddress != encryptedAddress)
                this.Apply(new UserAddressChanged(Guid.Parse(Id), encryptedAddress));
        }
        public void ChangePhone(AesKeyInfo keyInfo, Phone phone)
        {
            EncryptedPhone encryptedPhone = new EncryptedPhone(phone, keyInfo.Key, keyInfo.IV);
            if(this.EncPhone != encryptedPhone)
                this.Apply(new UserPhoneChanged(Guid.Parse(Id), encryptedPhone));
        }
        public void ChangeEmail(AesKeyInfo keyInfo, Email email)
        {
            EncryptedEmail encryptedEmail = new EncryptedEmail(email, keyInfo.Key, keyInfo.IV);
            if(this.EncEmail != encryptedEmail)
                this.Apply(new UserEmailChanged(Guid.Parse(Id), encryptedEmail));
        }
        public void ChangeAuth0Id(string auth0Id)
        {
            if(Auth0Id != auth0Id)
                this.Apply(new UserAuth0IdChanged(Guid.Parse(Id), auth0Id));
        }
         
        public void Activate()
        {
            if(!Active)
                this.Apply(new UserActivated(Guid.Parse(Id), this.EncDisplayName));
        }

        public void Deactivate()
        {
            if(Active)
                this.Apply(new UserDeactivated(Guid.Parse(Id), this.EncDisplayName));
        }
        
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        protected void When(UserRegistered @event)
        {
            Id = @event.Id.ToString();
            EncPersonName = @event.EncPersonName;
            EncDisplayName = @event.EncDisplayName;
            EncAddress = @event.EncAddress;
            EncPhone = @event.EncPhone;
            EncEmail = @event.EncEmail;
            Active = @event.Active;
        }
        protected void When(UserPersonNameChanged @event)
        {
            EncPersonName = @event.EncPersonName;
        }
        protected void When(UserDisplayNameChanged @event)
        {
            EncDisplayName = @event.EncDisplayName;
        }
        protected void When(UserAddressChanged @event)
        {
            EncAddress = @event.EncAddress;
        }
        protected void When(UserPhoneChanged @event)
        {
            EncPhone = @event.EncPhone;
        }
        protected void When(UserEmailChanged @event)
        {
            EncEmail = @event.EncEmail;
        }
        protected void When(UserAuth0IdChanged @event)
        {
            Auth0Id = @event.Auth0Id;
        }
        protected void When(UserActivated @event)
        {
            Active = true;
        }
        protected void When(UserDeactivated @event)
        {
            Active = false;
        }
        #endregion
    }
}