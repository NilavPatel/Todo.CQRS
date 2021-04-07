using System;
using System.Collections.Generic;
using Todo.Framework.Core.Event;

namespace Todo.Framework.Core.Aggregate
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        int Version { get; }
        IReadOnlyCollection<IEvent> DomainEvents { get; }

        void ApplyEvent(IEvent @event);

        void LoadFromHistory(IEnumerable<IEvent> history);
    }
}