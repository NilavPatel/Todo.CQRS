using System;
using System.Collections.Generic;
using Framework.Event;

namespace Framework.Aggregate
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