using System;
using System.Threading.Tasks;
using Todo.Framework.CommandBus;

namespace Todo.Framework.Aggregate
{
    public class AggregateHandler
    {
        private ICommandBus _bus;
        public AggregateHandler(ICommandBus bus)
        {
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task<ICommandResult> HandleCommand(IAggregate aggregate)
        {
            return await _bus.Submit(new SaveAggregateEvents() { Aggregate = aggregate });
        }
    }
}
