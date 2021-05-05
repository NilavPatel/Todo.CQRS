using System;
using System.Collections.Generic;
using Framework.Events;

namespace Framework.Aggregate
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        int Version { get; }
        IReadOnlyCollection<IEvent> DomainEvents { get; }

        void ApplyEvent(IEvent @event);

        void LoadFromHistory(IEnumerable<IEvent> history);

        void ClearDomainEvents();
    }
}