using System;
using Ezley.EventSourcing;

namespace Ezley.Events.Auth0User
{
    public class Auth0UserIdChanged:EventBase
    {
        public string Id { get; private set; }
        public Guid UserId { get; private set; }
        
        public Auth0UserIdChanged(string id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }   
    }
}