using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Aggregate;
using Framework.Event;
using Framework.Exceptions;

namespace Framework.UnitOfWork
{
    public class UowRepository : IUowRepository
    {
        private readonly Dictionary<Guid, AggregateRoot> _trackedAggregates;
        private IEventBus _bus;
        private IAggregateRepository _aggregateRepository;

        public UowRepository(IEventBus bus, IAggregateRepository aggregateRepository)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._aggregateRepository = aggregateRepository ?? throw new ArgumentNullException(nameof(aggregateRepository));
            _trackedAggregates = new Dictionary<Guid, AggregateRoot>();
        }

        public async Task<T> Get<T>(Guid id, int? version = null) where T : AggregateRoot
        {
            if (IsTracked(id))
            {
                var trackedAggregate = (T)_trackedAggregates[id];
                if (version != null && trackedAggregate.Version != version)
                {
                    throw new ConcurrencyException(trackedAggregate.Id);
                }
                return trackedAggregate;
            }

            var aggregate = await _aggregateRepository.Get<T>(id, version);
            if (version != null && aggregate.Version != version)
            {
                throw new ConcurrencyException(id);
            }
            await Add(aggregate);

            return aggregate;
        }

        public Task Add<T>(T aggregate) where T : AggregateRoot
        {
            if (!IsTracked(aggregate.Id))
            {
                _trackedAggregates.Add(aggregate.Id, aggregate);
            }
            else if (_trackedAggregates[aggregate.Id] != aggregate)
            {
                throw new ConcurrencyException(aggregate.Id);
            }
            return Task.FromResult(0);
        }

        public async Task Commit()
        {
            try
            {
                foreach (var aggregate in _trackedAggregates.Values)
                {
                    await _aggregateRepository.Save(aggregate);
                    await PublishEvents(aggregate.DomainEvents);
                    aggregate.ClearDomainEvents();
                }
            }
            finally
            {
                _trackedAggregates.Clear();
            }
        }

        private bool IsTracked(Guid id)
        {
            return _trackedAggregates.ContainsKey(id);
        }

        private async Task PublishEvents(IReadOnlyCollection<IEvent> events)
        {
            foreach (var @event in events)
            {
                await _bus.Publish(@event);
            }
        }
    }
}