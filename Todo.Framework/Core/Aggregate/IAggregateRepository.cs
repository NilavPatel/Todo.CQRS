using System;
using System.Threading.Tasks;

namespace Todo.Framework.Core.Aggregate
{
    public interface IAggregateRepository
    {
        Task Save<T>(T aggregate) where T : IAggregate;

        Task<T> Get<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregate;
    }
}
