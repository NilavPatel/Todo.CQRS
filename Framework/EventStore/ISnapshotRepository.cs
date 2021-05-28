using System;
using System.Threading.Tasks;

namespace Framework.EventStore
{
    public interface ISnapshotRepository
    {
        Task<SnapshotEntity> GetAsync(Guid aggregateId);
        Task SaveAsync(SnapshotEntity snapshot);
    }
}