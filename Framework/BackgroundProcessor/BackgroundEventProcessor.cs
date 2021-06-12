using System;
using System.Text;
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
            var settings = new CatchUpSubscriptionSettings(
                maxLiveQueueSize: 10000,
                readBatchSize: 500,
                verboseLogging: false,
                resolveLinkTos: true,
                subscriptionName: "mySubscription"
            );

            _eventStore.SubscribeToAllFrom(
                Position.Start,
                settings,
                eventAppeared: (sub, evt) => ProcessEvent(evt)
            );
        }

        private void ProcessEvent(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventStreamId.Contains("$"))
            {
                return;
            }
            var @event = TransformEvent(resolvedEvent);
            if (@event != null)
            {
                this._bus.PublishAsync(@event).ConfigureAwait(false);
            }
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