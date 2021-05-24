using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserEmailChanged : EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedEmail EncEmail { get; private set; }

        public UserEmailChanged(Guid id, EncryptedEmail encEmail)
        {
            Id = id;
            EncEmail = encEmail;
        }
    }
}