using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Aggregate;
using Framework.Events;
using Newtonsoft.Json;

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
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, _jsonSerializerSettings)),
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new EventMetadata() { FullName = e.GetType().FullName }, _jsonSerializerSettings))
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
            return page.Events.Select(e => TransformEvent(e));
        }

        private static IEvent TransformEvent(ResolvedEvent @event)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(@event.OriginalEvent.Data), _jsonSerializerSettings);
            var evt = o as IEvent;
            return evt;
        }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}