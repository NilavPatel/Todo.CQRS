using System;
using Todo.Framework.Core.Command;

namespace Todo.Framework.Core.CommandBus
{
    public class DefaultCommandBus : ICommandBus
    {
        IServiceProvider _serviceProvider;
        public DefaultCommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommandResult Submit<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }
            return ((ICommandHandler<TCommand>)handler).Handle(command);
        }
    }
}