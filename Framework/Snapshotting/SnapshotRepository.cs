using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Utils;
using Framework.Events;

namespace Framework.Snapshotting
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly IEventStoreConnection _eventStore;

        public SnapshotRepository(IEventStoreConnection eventStore)
        {
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task<Snapshot> GetAsync(Guid aggregateId)
        {
            var snapshotStreamName = GetSnapshotStreamName(aggregateId);
            var page = await this._eventStore.ReadStreamEventsBackwardAsync(snapshotStreamName, StreamPosition.End, 1, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return Serializer.Deserialize<Snapshot>(page.Events[0].OriginalEvent.Data);
        }

        public async Task SaveAsync(Snapshot snapshot)
        {
            var streamName = GetSnapshotStreamName(snapshot.Id);
            var data = new EventData(
                CombGuid.NewGuid(),
                snapshot.GetType().Name,
                true,
                Serializer.Serialize(snapshot),
                Serializer.Serialize(new EventMetadata() { FullName = snapshot.GetType().FullName })
            );
            await this._eventStore.AppendToStreamAsync(streamName, ExpectedVersion.Any, data);
        }

        private static string GetSnapshotStreamName(Guid subscriptionId) => $"snapshot_{subscriptionId}";
    }
}