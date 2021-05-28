using System;
using System.Threading.Tasks;

namespace Framework.Aggregate
{
    public interface IAggregateRepository
    {
        Task SaveAsync<T>(T aggregate) where T : IAggregateRoot;

        Task<T> GetAsync<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregateRoot;

        Task<bool> ExistAsync(Guid aggregateId);
    }
}
