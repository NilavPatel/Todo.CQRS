using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Aggregate;
using Framework.Events;
using Framework.Utils;

namespace Framework.EventStore
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventStoreConnection _eventStore;

        public EventRepository(IEventStoreConnection eventStore)
        {
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task SaveAsync(IAggregateRoot aggregate)
        {
            var data = aggregate.DomainEvents.Select(e =>
                new EventData(e.EventId,
                    e.GetType().Name,
                    true,
                    Serializer.Serialize(e),
                    Serializer.Serialize(new EventMetadata() { FullName = e.GetType().FullName })
                )
            );
            await this._eventStore.AppendToStreamAsync(aggregate.Id.ToString(), aggregate.Version - 1, data);
        }

        public async Task<IEnumerable<IEvent>> GetEvents(Guid aggregateId, int? startVersion = null)
        {
            var page = await this._eventStore.ReadStreamEventsForwardAsync(aggregateId.ToString(), startVersion ?? StreamPosition.Start, 4096, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return page.Events.Select(e => Serializer.TransformEvent(e.OriginalEvent.Data));
        }
    }
}