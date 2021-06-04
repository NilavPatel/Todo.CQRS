using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.EventStore
{
    public interface IEventRepository
    {
        Task<bool> SaveEventsAsync(IEnumerable<EventEntity> events);
        Task<IEnumerable<EventEntity>> GetEventsAsync(Guid aggregateId);
        Task<IEnumerable<EventEntity>> GetEventsFromVersionAsync(Guid aggregateId, int version);
        Task<bool> IsAnyEventExistAsync(Guid aggregateId);
        Task MarkEventAsSuccessAsync(Guid eventId);
    }
}