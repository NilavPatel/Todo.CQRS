using System.Threading.Tasks;
using Todo.Framework.Core.CommandBus;

namespace Todo.Framework.Core.Command
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<ICommandResult> Handle(TCommand command);
    }
}
