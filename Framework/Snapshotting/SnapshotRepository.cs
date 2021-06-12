using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Generators;
using Newtonsoft.Json;
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
            var snapshotStreamName = GetStreamName(aggregateId);
            var page = await this._eventStore.ReadStreamEventsBackwardAsync(snapshotStreamName, 0, 1, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return TransformSnapshot(page.Events[0]);
        }

        public async Task SaveAsync(Snapshot snapshot)
        {
            var streamName = GetStreamName(snapshot.Id);
            var data = new EventData(
                CombGuid.NewGuid(),
                snapshot.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot, _jsonSerializerSettings)),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new EventMetadata() { FullName = snapshot.GetType().FullName }, _jsonSerializerSettings))
            );
            await this._eventStore.AppendToStreamAsync(streamName, snapshot.Version - 1, data);
        }

        private string GetStreamName(Guid id) => $"Snapshot-{id}";

        private static Snapshot TransformSnapshot(ResolvedEvent @event)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(@event.OriginalEvent.Data), _jsonSerializerSettings);
            var snap = o as Snapshot;
            return snap;
        }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}