using Todo.Framework.Core.CommandBus;

namespace Todo.Framework.Core.Command
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        ICommandResult Handle(TCommand command);
    }
}
