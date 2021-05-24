using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserDisplayNameChanged : EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedDisplayName EncDisplayName { get; private set; }

        public UserDisplayNameChanged(Guid id, EncryptedDisplayName encryptedDisplayName)
        {
            Id = id;
            EncDisplayName = encryptedDisplayName;
        }
    }
}