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
        private string GetStreamName(Guid id) => $"Snapshot-{id}";

        public SnapshotRepository(IEventStoreConnection eventStore)
        {
            this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task<Snapshot> GetAsync(Guid aggregateId)
        {
            var snapshotStreamName = GetStreamName(aggregateId);
            var page = await this._eventStore.ReadStreamEventsBackwardAsync(snapshotStreamName, StreamPosition.End, 1, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return Serializer.TransformSnapshot(page.Events[0].OriginalEvent.Data);
        }

        public async Task SaveAsync(Snapshot snapshot)
        {
            var streamName = GetStreamName(snapshot.Id);
            var data = new EventData(
                CombGuid.NewGuid(),
                snapshot.GetType().Name,
                true,
                Serializer.Serialize(snapshot),
                Serializer.Serialize(new EventMetadata() { FullName = snapshot.GetType().FullName })
            );
            await this._eventStore.AppendToStreamAsync(streamName, ExpectedVersion.Any, data);
        }
    }
}