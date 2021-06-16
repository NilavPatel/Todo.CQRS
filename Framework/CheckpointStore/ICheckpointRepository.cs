using System.Threading.Tasks;

namespace Framework.CheckpointStore
{
    public interface ICheckpointRepository
    {
        Task<Checkpoint> GetCheckpoint(string key);
        Task SaveCheckpoint(Checkpoint checkpoint);
    }
}