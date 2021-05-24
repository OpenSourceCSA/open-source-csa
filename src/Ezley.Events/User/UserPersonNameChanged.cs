using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.Events
{
    public class UserPersonNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public EncryptedPersonName EncPersonName { get; private set; }

        public UserPersonNameChanged(Guid id, EncryptedPersonName encPersonName)
        {
            Id = id;
            EncPersonName = encPersonName;
        }
    }
}