using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.EventStore
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly EventStoreContext _dbContext;

        public SnapshotRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<SnapshotEntity> GetAsync(Guid aggregateId)
        {
            var snapshotEntity = await this._dbContext.Set<SnapshotEntity>()
                    .Where(x => x.AggregateId == aggregateId)
                    .OrderBy(e => e.AggregateVersion)
                    .LastOrDefaultAsync();

            return snapshotEntity;
        }

        public async Task SaveAsync(SnapshotEntity snapshot)
        {
            await this._dbContext.Set<SnapshotEntity>().AddAsync(snapshot);
            await this._dbContext.SaveChangesAsync();
        }
    }
}