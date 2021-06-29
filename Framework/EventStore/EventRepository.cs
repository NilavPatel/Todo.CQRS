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
            //Todo: add -2 to version as stream current version is -1.
            await this._eventStore.AppendToStreamAsync(aggregate.Id.ToString(), aggregate.Version - 2, data);
        }

        public async Task<IEnumerable<IEvent>> GetEvents(Guid aggregateId, int? startVersion = null)
        {
            var page = await this._eventStore.ReadStreamEventsForwardAsync(aggregateId.ToString(), startVersion != null ? (startVersion.Value - 1) : StreamPosition.Start, 4096, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            var events = page.Events.Select(e => Serializer.Deserialize<IEvent>(e.OriginalEvent.Data));
            return events;
        }
    }
}