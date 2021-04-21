using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Framework.Command;
using Framework.CommandBus;
using Framework.Event;

namespace Framework.Aggregate
{
    public class SaveAggregateEventsCommandHandler : ICommandHandler<SaveAggregateEvents>
    {
        private IEventBus _bus;
        private IAggregateRepository _aggregateRepository;

        public SaveAggregateEventsCommandHandler(IEventBus bus, IAggregateRepository aggregateRepository)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this._aggregateRepository = aggregateRepository ?? throw new ArgumentNullException(nameof(aggregateRepository));
        }

        public async Task<ICommandResult> Handle(SaveAggregateEvents command)
        {
            if (command.Aggregate.DomainEvents != null && command.Aggregate.DomainEvents.Any())
            {
                await this._aggregateRepository.Save(command.Aggregate);
                await PublishEvents(command.Aggregate.DomainEvents);
            }
            return new CommandResult(HttpStatusCode.OK, command.Aggregate.Id, command.Aggregate.Version, null);
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
