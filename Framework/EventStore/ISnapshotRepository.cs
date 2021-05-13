using System;
using System.Threading.Tasks;

namespace Framework.EventStore
{
    public interface ISnapshotRepository
    {
        Task<SnapshotEntity> Get(Guid aggregateId);
        Task Save(SnapshotEntity snapshot);
    }
}