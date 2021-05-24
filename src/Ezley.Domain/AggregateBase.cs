using System.Collections.Generic;
using Ezley.EventSourcing;

namespace ES.Domain
{
    public abstract class AggregateBase
    {
        public string Id { get; protected set; }
        public AggregateBase()
        {
        }

        public AggregateBase(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
                Version += 1;
            }
        }
        
        // Common Aggregate Properties
        public int Version { get; protected set; }

        public List<IEvent> Changes { get; protected set; } = new List<IEvent>();

        // Common Aggregate Functions
        protected void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
        }
        
        protected virtual void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
    }
}