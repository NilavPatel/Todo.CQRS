using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.CheckpointStore
{
    public class CheckpointRepository : ICheckpointRepository
    {
        private readonly CheckpointStoreContext _dbContext;
        public CheckpointRepository(CheckpointStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Checkpoint> GetCheckpoint(string key)
        {
            return await _dbContext.Set<Checkpoint>().FirstOrDefaultAsync();
        }

        public async Task SaveCheckpoint(Checkpoint checkpoint)
        {
            var oldCheckpoint = await _dbContext.Set<Checkpoint>().Where(x => x.Module == checkpoint.Module).FirstOrDefaultAsync();
            if (oldCheckpoint != null)
            {
                oldCheckpoint.Commit = checkpoint.Commit;
                oldCheckpoint.Prepare = checkpoint.Prepare;
                this._dbContext.Set<Checkpoint>().Update(oldCheckpoint);
            }
            else
            {
                await this._dbContext.Set<Checkpoint>().AddAsync(checkpoint);
            }

            await this._dbContext.SaveChangesAsync();
        }
    }
}