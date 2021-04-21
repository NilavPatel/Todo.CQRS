using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Framework.Event;
using Todo.Framework.Exceptions;

namespace Todo.Framework.Aggregate
{
    public class Aggregate : IAggregate
    {
        private List<IEvent> _domainEvents;
        public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public void ApplyEvent(IEvent @event)
        {
            _domainEvents = _domainEvents ?? new List<IEvent>();
            @event.Version = this.Version + 1;
            this.Mutate(@event);
            @event.SourceId = this.Id;
            @event.OccuredOn = DateTimeOffset.UtcNow;
            this._domainEvents.Add(@event);
        }

        private void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
            this.Version = @event.Version;
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history.ToArray())
            {
                if (e.Version != Version + 1)
                {
                    throw new AggregateOrEventMissingIdException(GetType(), e.GetType());
                }
                if (e.SourceId != Id && Id != default)
                {
                    throw new EventIdIncorrectException(e.SourceId, Id);
                }
                ((dynamic)this).When((dynamic)e);
                Id = e.SourceId;
                Version++;
            }
        }
    }
}