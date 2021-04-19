using System.Threading.Tasks;
using Todo.Framework.Core.Command;

namespace Todo.Framework.Core.CommandBus
{
    public interface ICommandBus
    {
        Task<ICommandResult> Submit<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
