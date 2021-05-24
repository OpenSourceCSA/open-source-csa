using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events.Auth0User;
using Ezley.EventSourcing;

namespace Csa.Domain.Auth
{
    public class Auth0User: AggregateBase
    {
        public Guid UserId { get; private set; }

        public Auth0User(IEnumerable<IEvent> events): base(events)
        {
        }
        public Auth0User(string id, Guid userId)
        {
            Apply(new Auth0UserRegistered(id, userId));
        }
        public void ChangeUserId(Guid userId)
        {
            if(UserId == Guid.Empty)
                this.Apply(new Auth0UserIdChanged(this.Id, userId));
        }
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        protected void When(Auth0UserRegistered @event)
        {
            Id = @event.Id;
            UserId = @event.UserId;
           
        }
        protected void When(Auth0UserIdChanged @event)
        {
            UserId = @event.UserId;
        }
        #endregion
    }
}