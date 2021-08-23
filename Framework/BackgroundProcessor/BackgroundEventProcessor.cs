using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.CheckpointStore;
using Framework.EventBus;
using Framework.Events;
using Framework.Utils;

namespace Framework.BackgroundProcessor
{
    public class BackgroundEventProcessor : IBackgroundEventProcessor
    {
        private readonly IIntegrationEventBus _bus;
        private readonly IEventStoreConnection _eventStore;
        private readonly ICheckpointRepository _checkpointStore;
        private string _subscriptionId;

        public BackgroundEventProcessor(IIntegrationEventBus bus, IEventStoreConnection eventStore, ICheckpointRepository checkpointStore)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            this._checkpointStore = checkpointStore ?? throw new ArgumentNullException(nameof(checkpointStore));
        }

        public async void Start(string subscriptionId)
        {
            this._subscriptionId = subscriptionId;
            var lastCheckpoint = await this._checkpointStore.GetCheckpoint(this._subscriptionId);

            this._eventStore.SubscribeToAllFrom(
                lastCheckpoint: lastCheckpoint != null ? new Position(lastCheckpoint.Value, lastCheckpoint.Value) : AllCheckpoint.AllStart,
                settings: CatchUpSubscriptionSettings.Default,
                eventAppeared: EventAppeared);
        }

        public async Task EventAppeared(EventStoreCatchUpSubscription _, ResolvedEvent resolvedEvent)
        {
            if (IsSystemStream(resolvedEvent.Event.EventStreamId))
            {
                return;
            }
            var @event = Serializer.Deserialize<IEvent>(resolvedEvent.OriginalEvent.Data);
            if (@event != null)
            {
                await this._bus.PublishAsync(@event);
            }
            await this._checkpointStore.SaveCheckpoint(this._subscriptionId, resolvedEvent.OriginalPosition.Value.CommitPosition);
        }

        private bool IsSystemStream(string linkedStream)
        {
            return linkedStream != null && (linkedStream.StartsWith("$") || linkedStream.StartsWith("Snapshot"));
        }
    }
}