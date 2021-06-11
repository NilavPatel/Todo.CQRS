using System;
using System.Threading.Tasks;

namespace Framework.Snapshotting
{
    public interface ISnapshotRepository
    {
        Task<Snapshot> GetAsync(Guid aggregateId);
        Task SaveAsync(Snapshot snapshot);
    }
}