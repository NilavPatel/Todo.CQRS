using System.Collections.Generic;
using System.Linq;
using System.Net;
using Todo.Framework.Core.CommandBus;
using Todo.Framework.Core.Event;
using Todo.Framework.Core.Repository;

namespace Todo.Framework.Core.Command
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

        public ICommandResult Handle(SaveEvents command)
        {
            if (command.Aggregate.DomainEvents != null && command.Aggregate.DomainEvents.Any())
            {
                this._aggregateRepository.Save(command.Aggregate);
                PublishEvents(command.Aggregate.DomainEvents);
            }
            return new CommandResult(HttpStatusCode.OK, command.Aggregate.Id, command.Aggregate.Version, null);
        }

        private void PublishEvents(IReadOnlyCollection<IEvent> events)
        {
            foreach (var @event in events)
            {
                _bus.Publish(@event);
            }
        }
    }
}
