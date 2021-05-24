using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserAddressChanged : EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedAddress EncAddress { get; private set; }

        public UserAddressChanged(Guid id, EncryptedAddress encAddress)
        {
            Id = id;
            EncAddress = encAddress;
        }
    }
}