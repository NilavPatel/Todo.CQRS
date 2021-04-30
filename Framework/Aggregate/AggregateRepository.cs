using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Event;
using Framework.EventStore;
using Framework.Exceptions;
using Framework.Generators;
using Framework.Snapshotting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Framework.Aggregate
{
    public class AggregateRepository : IAggregateRepository
    {
        protected readonly EventStoreContext _dbContext;
        protected readonly ISnapshotRepository _snapshotRepository;

        public AggregateRepository(EventStoreContext dbContext, ISnapshotRepository snapshotRepository)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._snapshotRepository = snapshotRepository;
        }

        public async Task<T> Get<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregateRoot
        {
            return await LoadAggregate<T>(aggregateId, aggregateVersion);
        }

        public async Task Save<T>(T aggregate) where T : IAggregateRoot
        {
            if (aggregate.DomainEvents == null)
            {
                return;
            }
            foreach (var e in aggregate.DomainEvents)
            {
                var eventEntity = new EventEntity
                {
                    Id = CombGuid.NewGuid(),
                    AggregateId = e.SourceId,
                    AggregateVersion = e.Version,
                    AggregateName = aggregate.GetType().FullName,
                    EventName = e.GetType().FullName,
                    Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, _jsonSerializerSettings)),
                    OccuredOn = e.OccuredOn
                };
                await _dbContext.Set<EventEntity>().AddAsync(eventEntity);
            }
            await _dbContext.SaveChangesAsync();

            if (SnapshotStrategy.ShouldMakeSnapShot(aggregate))
            {
                await SaveSnapshot(aggregate);
            }
        }

        private async Task<T> LoadAggregate<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregateRoot
        {
            if (aggregateVersion <= 0)
            {
                throw new AggregateVersionIncorrectException();
            }
            var aggregate = AggregateFactory<T>.CreateAggregate();

            int snapshotVersion = await RestoreAggregateFromSnapshot<T>(aggregateId, aggregate);
            if (snapshotVersion == -1)
            {
                var events = await _dbContext.Set<EventEntity>()
                                        .Where(e => e.AggregateId == aggregateId)
                                        .OrderBy(e => e.AggregateVersion)
                                        .Select(e => TransformEvent(e))
                                        .ToListAsync();

                if (!events.Any())
                {
                    throw new AggregateNotFoundException(typeof(T), aggregateId);
                }

                aggregate.LoadFromHistory(events);
                if (aggregateVersion != null && aggregate.Version != aggregateVersion)
                {
                    throw new ConcurrencyException(aggregateId);
                }
            }
            else
            {
                var events = await _dbContext.Set<EventEntity>()
                                        .Where(e => e.AggregateId == aggregateId && e.AggregateVersion > snapshotVersion)
                                        .OrderBy(e => e.AggregateVersion)
                                        .Select(e => TransformEvent(e))
                                        .ToListAsync();

                if (events.Any())
                {
                    aggregate.LoadFromHistory(events);
                    if (aggregateVersion != null && aggregate.Version != aggregateVersion)
                    {
                        throw new ConcurrencyException(aggregateId);
                    }
                }
            }

            return aggregate;
        }

        private static IEvent TransformEvent(EventEntity eventSelected)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(eventSelected.Data), _jsonSerializerSettings);
            var evt = o as IEvent;
            return evt;
        }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };

        private async Task SaveSnapshot(IAggregateRoot aggregate)
        {
            dynamic snapshot = ((dynamic)aggregate).GetSnapshot();
            await _snapshotRepository.Save(snapshot);
        }

        private async Task<int> RestoreAggregateFromSnapshot<T>(Guid id, IAggregateRoot aggregate)
        {
            if (!SnapshotStrategy.IsSnapshotable(aggregate.GetType()))
                return -1;
            var snapshot = await _snapshotRepository.Get(id);
            if (snapshot == null)
                return -1;
            ((dynamic)aggregate).Restore((dynamic)snapshot);
            return snapshot.Version;
        }
    }
}