using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Framework.EventBus;
using Framework.Events;
using Framework.EventStore;
using Framework.Exceptions;
using Framework.Snapshotting;

namespace Framework.Aggregate
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IDomainEventBus _bus;

        public AggregateRepository(ISnapshotRepository snapshotRepository, IEventRepository eventRepository, IDomainEventBus bus)
        {
            this._snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
            this._eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task<T> GetAsync<T>(Guid aggregateId, int? expectedVersion) where T : IAggregateRoot
        {
            return await LoadAggregate<T>(aggregateId, expectedVersion);
        }

        public async Task SaveAsync<T>(T aggregate) where T : IAggregateRoot
        {
            if (aggregate.DomainEvents == null)
            {
                return;
            }

            await this._eventRepository.SaveAsync(aggregate);

            if (SnapshotStrategy.ShouldMakeSnapshot(aggregate))
            {
                var snapshot = ((dynamic)aggregate).GetSnapshot();
                await this._snapshotRepository.SaveAsync(snapshot);
            }

            await PublishEvents(aggregate.DomainEvents);
            aggregate.ClearDomainEvents();
        }

        public async Task<bool> ExistAsync(Guid aggregateId)
        {
            var events = await this._eventRepository.GetEvents(aggregateId);
            return events != null ? true : false;
        }

        #region private methods
        private async Task<T> LoadAggregate<T>(Guid aggregateId, int? expectedVersion) where T : IAggregateRoot
        {
            if (expectedVersion < 1)
            {
                throw new AggregateVersionIncorrectException();
            }
            var aggregate = AggregateFactory<T>.CreateAggregate();

            int snapshotVersion = await RestoreAggregateFromSnapshot(aggregateId, aggregate);
            if (snapshotVersion != -1)
            {
                var remainingEvents = await this._eventRepository.GetEvents(aggregateId, snapshotVersion + 1);
                if (remainingEvents.Any())
                {
                    aggregate.LoadFromHistory(remainingEvents);
                }
                if (expectedVersion != null && aggregate.Version != expectedVersion)
                {
                    throw new ConcurrencyException(aggregateId);
                }
                return aggregate;
            }
            var allEvents = await this._eventRepository.GetEvents(aggregateId);
            if (!allEvents.Any())
            {
                throw new AggregateNotFoundException(typeof(T), aggregateId);
            }
            aggregate.LoadFromHistory(allEvents);
            if (expectedVersion != null && aggregate.Version != expectedVersion)
            {
                throw new ConcurrencyException(aggregateId);
            }
            return aggregate;
        }

        private async Task PublishEvents(IReadOnlyCollection<IEvent> events)
        {
            foreach (var @event in events)
            {
                await this._bus.PublishAsync(@event);
            }
        }

        private async Task<int> RestoreAggregateFromSnapshot(Guid aggregateId, IAggregateRoot aggregate)
        {
            if (!SnapshotStrategy.IsSnapshotable(aggregate.GetType()))
                return -1;
            var snapshot = await this._snapshotRepository.GetAsync(aggregateId);
            if (snapshot == null)
            {
                return -1;
            }
            ((dynamic)aggregate).Restore((dynamic)snapshot);
            return aggregate.Version;
        }
        #endregion
    }
}