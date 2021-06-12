using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Framework.Events;
using Framework.Aggregate;

namespace Framework.EventStore
{
    public interface IEventRepository
    {
        Task SaveAsync(IAggregateRoot aggregate);

        Task<IEnumerable<IEvent>> GetEvents(Guid aggregateId, int? startVersion = null);
    }
}