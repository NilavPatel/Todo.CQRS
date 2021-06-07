using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Aggregate;
using Framework.Exceptions;

namespace Framework.Session
{
    public class SessionRepository : ISessionRepository
    {
        private readonly Dictionary<Guid, AggregateRoot> _trackedAggregates;
        private readonly IAggregateRepository _aggregateRepository;

        public SessionRepository(IAggregateRepository aggregateRepository)
        {
            this._aggregateRepository = aggregateRepository ?? throw new ArgumentNullException(nameof(aggregateRepository));
            this._trackedAggregates = new Dictionary<Guid, AggregateRoot>();
        }

        public async Task<T> GetAsync<T>(Guid id, int? version = null) where T : AggregateRoot
        {
            if (IsTracked(id))
            {
                var trackedAggregate = (T)this._trackedAggregates[id];
                if (version != null && trackedAggregate.Version != version)
                {
                    throw new ConcurrencyException(trackedAggregate.Id);
                }
                return trackedAggregate;
            }

            var aggregate = await this._aggregateRepository.GetAsync<T>(id, version);
            if (version != null && aggregate.Version != version)
            {
                throw new ConcurrencyException(id);
            }
            await AddAsync(aggregate);

            return aggregate;
        }

        public Task AddAsync<T>(T aggregate) where T : AggregateRoot
        {
            if (!IsTracked(aggregate.Id))
            {
                this._trackedAggregates.Add(aggregate.Id, aggregate);
            }
            else if (this._trackedAggregates[aggregate.Id] != aggregate)
            {
                throw new ConcurrencyException(aggregate.Id);
            }
            return Task.FromResult(0);
        }

        public async Task CommitAsync()
        {
            try
            {
                foreach (var aggregate in this._trackedAggregates.Values)
                {
                    await this._aggregateRepository.SaveAsync(aggregate);
                }
            }
            finally
            {
                this._trackedAggregates.Clear();
            }
        }

        private bool IsTracked(Guid id)
        {
            return this._trackedAggregates.ContainsKey(id);
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            return await this._aggregateRepository.ExistAsync(id);
        }
    }
}