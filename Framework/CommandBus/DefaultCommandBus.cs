using System;
using System.Threading.Tasks;
using Framework.Command;

namespace Framework.CommandBus
{
    public class DefaultCommandBus : ICommandBus
    {
        IServiceProvider _serviceProvider;
        public DefaultCommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<ICommandResult> Submit<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }
            return await ((ICommandHandler<TCommand>)handler).Handle(command);
        }
    }
}