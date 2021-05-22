using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.EventStore
{
    public class Eventrepository : IEventrepository
    {
        private readonly EventStoreContext _dbContext;

        public Eventrepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<EventEntity>> GetEvents(Guid aggregateId)
        {
            return await _dbContext.Set<EventEntity>()
                            .Where(e => e.AggregateId == aggregateId)
                            .OrderBy(e => e.AggregateVersion)
                            .ToListAsync();
        }

        public async Task<IEnumerable<EventEntity>> GetEventsFromVersion(Guid aggregateId, int version)
        {
            return await _dbContext.Set<EventEntity>()
                            .Where(e => e.AggregateId == aggregateId && e.AggregateVersion > version)
                            .OrderBy(e => e.AggregateVersion)
                            .ToListAsync();
        }

        public async Task<bool> SaveEvents(IEnumerable<EventEntity> events)
        {
            foreach (var eve in events)
            {
                await _dbContext.Set<EventEntity>().AddAsync(eve);
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsAnyEventExist(Guid aggregateId)
        {
            return await _dbContext.Set<EventEntity>()
                .AnyAsync(e => e.AggregateId == aggregateId);
        }

        public async Task MarkEventAsSuccess(Guid eventId)
        {
            var eve = await _dbContext.Set<EventEntity>()
                            .FirstAsync(e => e.EventId == eventId);
            eve.Success = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}