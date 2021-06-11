using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework.Generators;
using Newtonsoft.Json;

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
            var snapshotStreamId = GetSnapshotStream(aggregateId);
            var page = await this._eventStore.ReadStreamEventsForwardAsync(snapshotStreamId, StreamPosition.End, 1, false);
            if (page.Status == SliceReadStatus.StreamNotFound)
            {
                return null;
            }
            return TransformSnapshot(page.Events[0]);
        }

        public async Task SaveAsync(Snapshot snapshot)
        {
            var data = new EventData(CombGuid.NewGuid(), snapshot.GetType().FullName, false, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot, _jsonSerializerSettings)), null);
            await this._eventStore.AppendToStreamAsync(GetSnapshotStream(snapshot.Id), snapshot.Version, data);
        }

        private string GetSnapshotStream(Guid id) => $"Snapshot-{id}";

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