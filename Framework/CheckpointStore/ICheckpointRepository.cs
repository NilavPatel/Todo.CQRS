using System.Threading.Tasks;

namespace Framework.CheckpointStore
{
    public interface ICheckpointRepository
    {
        Task<long?> GetCheckpoint(string subscriptionId);
        Task SaveCheckpoint(string subscriptionId, long position);
    }
}