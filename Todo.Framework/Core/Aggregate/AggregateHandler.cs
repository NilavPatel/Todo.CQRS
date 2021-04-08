using System;
using Todo.Framework.Core.Command;
using Todo.Framework.Core.CommandBus;

namespace Todo.Framework.Core.Aggregate
{
    public class AggregateHandler
    {
        private ICommandBus _bus;
        public AggregateHandler(ICommandBus bus)
        {
            this._bus = bus;
        }

        public ICommandResult HandleCommand(IAggregateRoot aggregate)
        {
            return _bus.Submit(new SaveEvents() { Aggregate = aggregate });
        }
    }
}
