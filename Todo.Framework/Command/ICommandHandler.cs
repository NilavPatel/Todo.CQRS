using System.Threading.Tasks;
using Todo.Framework.CommandBus;

namespace Todo.Framework.Command
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<ICommandResult> Handle(TCommand command);
    }
}
