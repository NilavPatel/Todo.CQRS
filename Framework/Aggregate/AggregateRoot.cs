using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Events;
using Framework.Exceptions;
using Framework.Generators;

namespace Framework.Aggregate
{
    public class AggregateRoot : IAggregateRoot
    {
        private List<IEvent> _domainEvents = new List<IEvent>();
        public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public void ApplyEvent(IEvent @event)
        {
            lock (_domainEvents)
            {
                @event.EventId = CombGuid.NewGuid();
                @event.Version = this.Version + 1;
                this.Mutate(@event);
                @event.SourceId = this.Id;
                @event.OccuredOn = DateTimeOffset.UtcNow;
                this._domainEvents.Add(@event);
            }
        }

        private void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
            this.Version = @event.Version;
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            lock (_domainEvents)
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

        public void ClearDomainEvents()
        {
            lock (_domainEvents)
            {
                if (this.DomainEvents == null)
                {
                    return;
                }
                this._domainEvents.Clear();
            }
        }
    }
}