using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Events;
using Framework.EventStore;
using Framework.Exceptions;
using Framework.Generators;
using Framework.Snapshotting;
using Newtonsoft.Json;

namespace Framework.Aggregate
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IEventrepository _eventrepository;
        private readonly IEventBus _bus;

        public AggregateRepository(ISnapshotRepository snapshotRepository, IEventrepository eventrepository, IEventBus bus)
        {
            this._snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
            this._eventrepository = eventrepository ?? throw new ArgumentNullException(nameof(eventrepository));
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task<T> GetAsync<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregateRoot
        {
            return await LoadAggregate<T>(aggregateId, aggregateVersion);
        }

        public async Task SaveAsync<T>(T aggregate) where T : IAggregateRoot
        {
            if (aggregate.DomainEvents == null)
            {
                return;
            }

            var events = aggregate.DomainEvents.Select(e => new EventEntity
            {
                EventId = e.EventId,
                AggregateId = e.SourceId,
                AggregateVersion = e.Version,
                AggregateName = aggregate.GetType().FullName,
                EventName = e.GetType().FullName,
                Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, _jsonSerializerSettings)),
                OccuredOn = e.OccuredOn
            });

            await _eventrepository.SaveEventsAsync(events);

            if (SnapshotStrategy.ShouldMakeSnapshot(aggregate))
            {
                await SaveSnapshot(aggregate);
            }

            await PublishEvents(aggregate.DomainEvents);
            aggregate.ClearDomainEvents();
        }

        public async Task<bool> ExistAsync(Guid aggregateId)
        {
            return await _eventrepository.IsAnyEventExistAsync(aggregateId);
        }

        #region private methods
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
                var eventEntities = await _eventrepository.GetEventsAsync(aggregateId);
                if (!eventEntities.Any())
                {
                    throw new AggregateNotFoundException(typeof(T), aggregateId);
                }

                var events = eventEntities.Select(e => TransformEvent(e));
                aggregate.LoadFromHistory(events);
                if (aggregateVersion != null && aggregate.Version != aggregateVersion)
                {
                    throw new ConcurrencyException(aggregateId);
                }
            }
            else
            {
                var eventEntities = await _eventrepository.GetEventsFromVersionAsync(aggregateId, snapshotVersion);
                if (eventEntities.Any())
                {
                    var events = eventEntities.Select(e => TransformEvent(e));
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

        private static Snapshot TransformSnapshot(SnapshotEntity snapShotEntity)
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

        private async Task PublishEvents(IReadOnlyCollection<IEvent> events)
        {
            if (events == null)
            {
                return;
            }
            foreach (var @event in events)
            {
                await _bus.PublishAsync(@event);
            }
        }
        private async Task SaveSnapshot(IAggregateRoot aggregate)
        {
            dynamic snapshot = ((dynamic)aggregate).GetSnapshot();
            var snapshotEntity = new SnapshotEntity
            {
                SnapshotId = CombGuid.NewGuid(),
                AggregateId = snapshot.Id,
                AggregateVersion = snapshot.Version,
                SnapshotName = snapshot.GetType().FullName,
                Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot, _jsonSerializerSettings)),
                CreatedOn = DateTime.Now
            };
            await _snapshotRepository.SaveAsync(snapshotEntity);
        }

        private async Task<int> RestoreAggregateFromSnapshot<T>(Guid id, IAggregateRoot aggregate)
        {
            if (!SnapshotStrategy.IsSnapshotable(aggregate.GetType()))
                return -1;
            var snapshotEntity = await _snapshotRepository.GetAsync(id);
            if (snapshotEntity == null)
            {
                return -1;
            }
            var snapshot = TransformSnapshot(snapshotEntity);
            ((dynamic)aggregate).Restore((dynamic)snapshot);
            return snapshot.Version;
        }
        #endregion
    }
}