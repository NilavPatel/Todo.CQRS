using System;
using System.Threading.Tasks;

namespace Framework.Snapshotting
{
    public interface ISnapshotRepository
    {
        Task<Snapshot> Get(Guid id);
        Task Save(Snapshot snapshot);
    }
}