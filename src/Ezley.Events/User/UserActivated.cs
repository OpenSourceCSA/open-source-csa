using System;
using Azure.Core.Cryptography;
using Ezley.EventSourcing;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserActivated : EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedDisplayName EncDisplayName { get; private set; }

        public UserActivated(Guid id, EncryptedDisplayName encDisplayName)
        {
            Id = id;
            EncDisplayName = encDisplayName;
        }
    }
}