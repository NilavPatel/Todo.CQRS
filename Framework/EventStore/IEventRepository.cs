using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.EventStore
{
    public interface IEventrepository
    {
        Task<bool> SaveEvents(IEnumerable<EventEntity> events);
        Task<IEnumerable<EventEntity>> GetEvents(Guid aggregateId);
        Task<IEnumerable<EventEntity>> GetEventsFromVersion(Guid aggregateId, int version);
        Task<bool> IsAnyEventExist(Guid aggregateId);
        Task MarkEventAsSuccess(Guid eventId);
    }
}