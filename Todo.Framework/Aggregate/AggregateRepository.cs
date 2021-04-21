using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Todo.Framework.Event;
using Todo.Framework.EventStore;
using Todo.Framework.Exceptions;

namespace Todo.Framework.Aggregate
{
    public class AggregateRepository : IAggregateRepository
    {
        protected readonly EventStoreContext _dbContext;

        public AggregateRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> Get<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregate
        {
            return await LoadAggregate<T>(aggregateId, aggregateVersion);
        }

        public async Task Save<T>(T aggregate) where T : IAggregate
        {
            foreach (var e in aggregate.DomainEvents)
            {
                var eventEntity = new EventEntity
                {
                    Id = Guid.NewGuid(),
                    AggregateName = aggregate.GetType().Name,
                    AggregateId = e.SourceId,
                    AggregateVersion = e.Version,
                    Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, _jsonSerializerSettings)),
                    EventName = e.GetType().Name,
                    EventFullName = Encoding.UTF8.GetBytes(e.GetType().FullName),
                    TimeStamp = DateTimeOffset.UtcNow
                };
                await _dbContext.Set<EventEntity>().AddAsync(eventEntity);
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task<T> LoadAggregate<T>(Guid aggregateId, int? aggregateVersion) where T : IAggregate
        {
            if (aggregateVersion <= 0)
            {
                throw new AggregateVersionIncorrectException();
            }

            var events = await _dbContext.Set<EventEntity>()
                                .Where(e => e.AggregateId == aggregateId)
                                .OrderBy(e => e.AggregateVersion)
                                .Select(e => TransformEvent(e))
                                .ToListAsync();

            if (!events.Any())
            {
                throw new AggregateNotFoundException(typeof(T), aggregateId);
            }

            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.LoadFromHistory((IEnumerable<IEvent>)events);
            if (aggregateVersion != null && aggregate.Version != aggregateVersion)
            {
                throw new ConcurrencyException(aggregateId);
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

    }
}