using System;

namespace Todo.Framework.Core.Aggregate
{
    public interface IAggregateRepository
    {
        void Save<T>(T aggregate) where T : IAggregate;

        T Get<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregate;
    }
}
