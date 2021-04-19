using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Todo.Framework.Core.Command;
using Todo.Framework.Core.CommandBus;
using Todo.Framework.Core.Event;

namespace Todo.Framework.Core.Aggregate
{
    public class SaveEventsHandler : ICommandHandler<SaveEvents>
    {
        private IEventBus _bus;
        private IAggregateRepository _aggregateRepository;

        public SaveEventsHandler(IEventBus bus, IAggregateRepository aggregateRepository)
        {
            this._bus = bus;
            this._aggregateRepository = aggregateRepository;
        }

        public async Task<ICommandResult> Handle(SaveEvents command)
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
