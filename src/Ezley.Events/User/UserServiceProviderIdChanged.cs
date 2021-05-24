using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserServiceProviderIdChanged : EventBase
    {
        public Guid Id { get; private set; }
        public Guid ServiceProviderId { get; private set; }

        public UserServiceProviderIdChanged(Guid id, Guid serviceProviderId)
        {
            Id = id;
            ServiceProviderId = serviceProviderId;
        }
    }
}