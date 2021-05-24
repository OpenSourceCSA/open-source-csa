using System;
using Ezley.EventSourcing;

namespace Ezley.Events.Auth0User
{
    public class Auth0UserRegistered:EventBase
    {
        public string Id { get; private set; }
        public Guid UserId { get; private set; }
        
        public Auth0UserRegistered(string id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }   
    }
}