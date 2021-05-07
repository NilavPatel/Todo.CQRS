using System;
using System.Threading.Tasks;

namespace Framework.EventStore
{
    public interface ISnapshotRepository
    {
        Task<SnapShotEntity> Get(Guid aggregateId);
        Task Save(SnapShotEntity snapshot);
    }
}