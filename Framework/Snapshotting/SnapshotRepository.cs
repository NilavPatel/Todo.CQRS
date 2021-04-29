using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.EventStore;
using Framework.Generators;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Framework.Snapshotting
{
    public class SnapshotRepository : ISnapshotRepository
    {
        protected readonly EventStoreContext _dbContext;

        public SnapshotRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Snapshot> Get(Guid id)
        {
            var snapshotEntity = await _dbContext.Set<SnapShotEntity>()
                .Where(x => x.Id == id)
                .OrderBy(e => e.AggregateVersion)
                .Select(e => TransformSnapshot(e))
                .LastOrDefaultAsync();

            return snapshotEntity;
        }

        public async Task Save(Snapshot snapshot)
        {
            var snapshotEntity = new SnapShotEntity
            {
                Id = CombGuid.NewGuid(),
                AggregateId = snapshot.Id,
                AggregateVersion = snapshot.Version,
                SnapshotName = snapshot.GetType().FullName,
                Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot, _jsonSerializerSettings)),
                CreatedOn = DateTime.Now
            };

            await _dbContext.Set<SnapShotEntity>().AddAsync(snapshotEntity);

            await _dbContext.SaveChangesAsync();
        }

        private static Snapshot TransformSnapshot(SnapShotEntity snapShotEntity)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(snapShotEntity.Data), _jsonSerializerSettings);
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