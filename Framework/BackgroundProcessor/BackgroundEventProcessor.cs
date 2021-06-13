using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.EventBus;
using Framework.Events;
using Newtonsoft.Json;

namespace Framework.BackgroundProcessor
{
    public class BackgroundEventProcessor : IBackgroundEventProcessor
    {
        private readonly IIntegrationEventBus _bus;
        private readonly IEventStoreConnection _eventStore;

        public BackgroundEventProcessor(IIntegrationEventBus bus, IEventStoreConnection eventStore)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public void Start()
        {
            //var lastCheckpoint = checkpointStore.Load("todo-checkpoint");

            _eventStore.SubscribeToAllFrom(
                lastCheckpoint: AllCheckpoint.AllStart,
                settings: CatchUpSubscriptionSettings.Default,
                eventAppeared: EventAppeared);
        }

        public Task EventAppeared(EventStoreCatchUpSubscription _, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventStreamId.Contains("$"))
            {
                return Task.CompletedTask;
            }
            var @event = TransformEvent(resolvedEvent);
            if (@event != null)
            {
                this._bus.PublishAsync(@event).ConfigureAwait(false);
            }
            //checkpointStore.Save(resolvedEvent.OriginalPosition.Value);
            return Task.CompletedTask;
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