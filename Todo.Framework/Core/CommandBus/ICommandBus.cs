using Todo.Framework.Core.Command;

namespace Todo.Framework.Core.CommandBus
{
    public interface ICommandBus
    {
        ICommandResult Submit<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
