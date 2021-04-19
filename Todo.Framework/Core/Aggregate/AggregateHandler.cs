using System.Threading.Tasks;
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

        public async Task<ICommandResult> HandleCommand(IAggregate aggregate)
        {
            return await _bus.Submit(new SaveEvents() { Aggregate = aggregate });
        }
    }
}
