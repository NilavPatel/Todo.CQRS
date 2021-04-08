using System;
using Todo.Framework.Core.Aggregate;

namespace Todo.Framework.Core.Repository
{
    public interface IAggregateRepository
    {
        void Save<T>(T aggregate) where T : IAggregateRoot;

        T Get<T>(Guid aggregateId) where T : IAggregateRoot;

        T Get<T>(Guid aggregateId, int version) where T : IAggregateRoot;
    }
}
