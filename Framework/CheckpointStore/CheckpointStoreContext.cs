using Microsoft.EntityFrameworkCore;

namespace Framework.CheckpointStore
{
    public class CheckpointStoreContext : DbContext
    {
        public CheckpointStoreContext(DbContextOptions<CheckpointStoreContext> options)
          : base(options)
        { }

        public DbSet<Checkpoint> Checkpoints { get; set; }
    }
}