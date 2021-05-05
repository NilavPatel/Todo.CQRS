using System;
using System.Threading.Tasks;

namespace Framework.Aggregate
{
    public interface IAggregateRepository
    {
        Task Save<T>(T aggregate) where T : IAggregateRoot;

        Task<T> Get<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregateRoot;

        Task<bool> Exist(Guid aggregateId);
    }
}
