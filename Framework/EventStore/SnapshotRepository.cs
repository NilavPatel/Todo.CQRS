using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.EventStore
{
    public class SnapshotRepository : ISnapshotRepository
    {
        protected readonly EventStoreContext _dbContext;

        public SnapshotRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<SnapShotEntity> Get(Guid aggregateId)
        {
            var snapshotEntity = await _dbContext.Set<SnapShotEntity>()
                    .Where(x => x.AggregateId == aggregateId)
                    .OrderBy(e => e.AggregateVersion)
                    .LastOrDefaultAsync();

            return snapshotEntity;
        }

        public async Task Save(SnapShotEntity snapshot)
        {
            await _dbContext.Set<SnapShotEntity>().AddAsync(snapshot);
            await _dbContext.SaveChangesAsync();
        }
    }
}