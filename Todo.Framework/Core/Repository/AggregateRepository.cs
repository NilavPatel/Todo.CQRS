using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Todo.Framework.Core.Aggregate;
using Todo.Framework.Core.Event;
using Todo.Framework.Core.EventStore;

namespace Todo.Framework.Core.Repository
{
    public class AggregateRepository : IAggregateRepository
    {
        protected readonly EventStoreContext _dbContext;

        public AggregateRepository(EventStoreContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public T Get<T>(Guid aggregateId) where T : IAggregateRoot
        {
            return LoadAggregate<T>(aggregateId, Int32.MaxValue);
        }

        public T Get<T>(Guid aggregateId, int version) where T : IAggregateRoot
        {
            return LoadAggregate<T>(aggregateId, version);
        }

        public void Save<T>(T aggregate) where T : IAggregateRoot
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
                _dbContext.Set<EventEntity>().Add(eventEntity);
            }
            _dbContext.SaveChanges();
        }

        private T LoadAggregate<T>(Guid id, int version) where T : IAggregateRoot
        {
            if (version <= 0)
            {
                throw new Exception("Aggregate version should not be less then or equal to 0");
            }

            var events = _dbContext.Set<EventEntity>()
                                .Where(e => e.AggregateId == id && e.AggregateVersion <= version)
                                .OrderBy(e => e.AggregateVersion)
                                .Select(e => TransformEvent(e))
                                .ToList();

            if (!events.Any())
            {
                throw new Exception($"Aggregate {id} of type {typeof(T).FullName} was not found");
            }

            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.LoadFromHistory((IEnumerable<IEvent>)events);
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