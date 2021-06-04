using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.EventStore
{
    public class EventRepository : IEventRepository
    {
        private readonly EventStoreContext _dbContext;

        public EventRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<EventEntity>> GetEventsAsync(Guid aggregateId)
        {
            return await _dbContext.Set<EventEntity>()
                            .Where(e => e.AggregateId == aggregateId)
                            .OrderBy(e => e.AggregateVersion)
                            .ToListAsync();
        }

        public async Task<IEnumerable<EventEntity>> GetEventsFromVersionAsync(Guid aggregateId, int version)
        {
            return await _dbContext.Set<EventEntity>()
                            .Where(e => e.AggregateId == aggregateId && e.AggregateVersion > version)
                            .OrderBy(e => e.AggregateVersion)
                            .ToListAsync();
        }

        public async Task<bool> SaveEventsAsync(IEnumerable<EventEntity> events)
        {
            foreach (var eve in events)
            {
                await _dbContext.Set<EventEntity>().AddAsync(eve);
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsAnyEventExistAsync(Guid aggregateId)
        {
            return await _dbContext.Set<EventEntity>()
                .AnyAsync(e => e.AggregateId == aggregateId);
        }

        public async Task MarkEventAsSuccessAsync(Guid eventId)
        {
            var eve = await _dbContext.Set<EventEntity>()
                            .FirstAsync(e => e.EventId == eventId);
            eve.Success = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}