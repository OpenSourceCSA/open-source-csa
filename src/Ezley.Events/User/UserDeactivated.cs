using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserDeactivated : EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedDisplayName EncDisplayName { get; private set; }

        public UserDeactivated(Guid id, EncryptedDisplayName encDisplayName)
        {
            Id = id;
            EncDisplayName = encDisplayName;
        }
    }
}