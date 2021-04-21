using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Framework.CommandBus;
using Framework.Event;

namespace Framework.Aggregate
{
    public class AggregateHandler
    {
        private IEventBus _bus;
        private IAggregateRepository _aggregateRepository;

        public AggregateHandler(IEventBus bus, IAggregateRepository aggregateRepository)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._aggregateRepository = aggregateRepository ?? throw new ArgumentNullException(nameof(aggregateRepository));
        }

        public async Task<ICommandResult> HandleCommand(IAggregate aggregate)
        {
            if (aggregate.DomainEvents != null && aggregate.DomainEvents.Any())
            {
                await this._aggregateRepository.Save(aggregate);
                await PublishEvents(aggregate.DomainEvents);
            }
            return new CommandResult(HttpStatusCode.OK, aggregate.Id, aggregate.Version, null);
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
