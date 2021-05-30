using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

// ReSharper disable once CheckNamespace
namespace Ezley.Events
{
    public class UserRegistered: EventBase
    {
        public Guid Id { get; private set; }
        public string Auth0Id { get; private set; }
        public EncryptedPersonName EncPersonName { get; private set; }
        public EncryptedDisplayName EncDisplayName { get; private set; }
        public EncryptedAddress EncAddress { get; private set; }
        public EncryptedPhone EncPhone { get; private set; }
        public EncryptedEmail EncEmail { get; private set; }
        public bool Active { get; private set; }

         
        public UserRegistered(
            Guid id, 
            string auth0Id,
            EncryptedPersonName encPersonName, 
            EncryptedDisplayName encDisplayName,
            EncryptedAddress encAddress, 
            EncryptedPhone encPhone, 
            EncryptedEmail encEmail, 
            bool active)
        {
            Id = id;
            Auth0Id = auth0Id;
            EncPersonName = encPersonName;
            EncDisplayName = encDisplayName;
            EncAddress = encAddress;
            EncPhone = encPhone;
            EncEmail = encEmail;
            Active = active;
        }
    }
}