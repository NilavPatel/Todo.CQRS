using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Events;
using Framework.Utils;
using Microsoft.EntityFrameworkCore;

namespace Framework.CheckpointStore
{
    public class CheckpointRepository : ICheckpointRepository
    {
        private readonly IEventStoreConnection _eventStore;

        public CheckpointRepository(IEventStoreConnection eventStore)
        {
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task<long?> GetCheckpoint(string subscriptionId)
        {
            var streamName = GetCheckpointStreamName(subscriptionId);

            var page = await this._eventStore.ReadStreamEventsBackwardAsync(streamName, StreamPosition.End, 1, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return Serializer.Deserialize<Checkpoint>(page.Events[0].OriginalEvent.Data).Position;
        }

        public async Task SaveCheckpoint(string subscriptionId, long position)
        {
            var checkpoint = new Checkpoint() { SubscriptionId = subscriptionId, Position = position };
            var streamName = GetCheckpointStreamName(subscriptionId);
            var data = new EventData(
                CombGuid.NewGuid(),
                checkpoint.GetType().Name,
                true,
                Serializer.Serialize(checkpoint),
                Serializer.Serialize(new EventMetadata() { FullName = checkpoint.GetType().FullName })
            );
            await this._eventStore.AppendToStreamAsync(streamName, ExpectedVersion.Any, data);
        }

        private static string GetCheckpointStreamName(string subscriptionId) => $"checkpoint_{subscriptionId}";
    }
}