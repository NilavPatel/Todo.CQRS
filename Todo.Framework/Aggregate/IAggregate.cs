using System;
using System.Collections.Generic;
using Todo.Framework.Event;

namespace Todo.Framework.Aggregate
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }
        IReadOnlyCollection<IEvent> DomainEvents { get; }

        void ApplyEvent(IEvent @event);

        void LoadFromHistory(IEnumerable<IEvent> history);
    }
}