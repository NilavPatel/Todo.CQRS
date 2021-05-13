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

        public async Task<SnapshotEntity> Get(Guid aggregateId)
        {
            var snapshotEntity = await _dbContext.Set<SnapshotEntity>()
                    .Where(x => x.AggregateId == aggregateId)
                    .OrderBy(e => e.AggregateVersion)
                    .LastOrDefaultAsync();

            return snapshotEntity;
        }

        public async Task Save(SnapshotEntity snapshot)
        {
            await _dbContext.Set<SnapshotEntity>().AddAsync(snapshot);
            await _dbContext.SaveChangesAsync();
        }
    }
}