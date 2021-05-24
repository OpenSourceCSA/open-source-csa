using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserPhoneChanged:EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedPhone EncPhone { get; private set; }

        public UserPhoneChanged(Guid id, EncryptedPhone encPhone)
        {
            Id = id;
            EncPhone = encPhone;
        }
    }
}