using System.Threading.Tasks;
using Todo.Framework.Command;

namespace Todo.Framework.CommandBus
{
    public interface ICommandBus
    {
        Task<ICommandResult> Submit<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
