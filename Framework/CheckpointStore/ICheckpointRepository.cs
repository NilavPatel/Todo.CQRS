using System.Threading.Tasks;

namespace Framework.CheckpointStore
{
    public interface ICheckpointRepository
    {
        Task<Checkpoint> GetCheckpoint(string subscriptionId);
        Task SaveCheckpoint(Checkpoint checkpoint);
    }
}