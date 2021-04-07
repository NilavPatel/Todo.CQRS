using System;
using System.Net;
using Todo.Framework.Core.Command;
using Todo.Framework.Core.CommandBus;
using Todo.Framework.Core.Repository;

namespace Todo.Framework.Core.Aggregate
{
    public class AggregateHandler
    {
        private ICommandBus _bus;
        private IAggregateRepository _repository;
        public AggregateHandler(ICommandBus bus, IAggregateRepository repository)
        {
            this._bus = bus;
            this._repository = repository;
        }

        public ICommandResult HandleCommand(IAggregateRoot aggregate, Guid? entityId = null)
        {
            return _bus.Submit(new SaveEvents() { Aggregate = aggregate });
        }
    }
}
