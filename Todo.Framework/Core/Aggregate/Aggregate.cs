using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Framework.Core.Event;

namespace Todo.Framework.Core.Aggregate
{
    public class Aggregate : IAggregateRoot
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
                    throw new Exception($"An event of type {e.GetType().FullName} was tried saved from {GetType().FullName} but no id where set on either");
                }
                if (e.SourceId != Id && Id != default)
                {
                    throw new Exception($"Event {e.SourceId} has a different Id from it's aggregates Id ({Id})");
                }
                ((dynamic)this).When((dynamic)e);
                Id = e.SourceId;
                Version++;
            }
        }
    }
}