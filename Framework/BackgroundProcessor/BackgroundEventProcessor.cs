using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.CheckpointStore;
using Framework.EventBus;
using Framework.Utils;

namespace Framework.BackgroundProcessor
{
    public class BackgroundEventProcessor : IBackgroundEventProcessor
    {
        private readonly IIntegrationEventBus _bus;
        private readonly IEventStoreConnection _eventStore;
        private readonly ICheckpointRepository _checkpointStore;
        private string _module;

        public BackgroundEventProcessor(IIntegrationEventBus bus, IEventStoreConnection eventStore, ICheckpointRepository checkpointStore)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            this._checkpointStore = checkpointStore ?? throw new ArgumentNullException(nameof(checkpointStore));
        }

        public async void Start(string module)
        {
            _module = module;
            var lastCheckpoint = await _checkpointStore.GetCheckpoint(_module);

            _eventStore.SubscribeToAllFrom(
                lastCheckpoint: lastCheckpoint != null ? new Position(lastCheckpoint.Commit, lastCheckpoint.Prepare) : AllCheckpoint.AllStart,
                settings: CatchUpSubscriptionSettings.Default,
                eventAppeared: EventAppeared);
        }

        public async Task EventAppeared(EventStoreCatchUpSubscription _, ResolvedEvent resolvedEvent)
        {
            if (IsSystemStream(resolvedEvent.Event.EventStreamId))
            {
                return;
            }
            var @event = Serializer.TransformEvent(resolvedEvent.OriginalEvent.Data);
            if (@event != null)
            {
                await this._bus.PublishAsync(@event);
            }
            await _checkpointStore.SaveCheckpoint(new Checkpoint
            {
                Module = _module,
                Commit = resolvedEvent.OriginalPosition.Value.CommitPosition,
                Prepare = resolvedEvent.OriginalPosition.Value.PreparePosition
            });
        }

        private bool IsSystemStream(string linkedStream)
        {
            return linkedStream != null && linkedStream.StartsWith("$");
        }
    }
}