using System.Threading.Tasks;
using Framework.Command;

namespace Framework.CommandBus
{
    public interface ICommandBus
    {
        Task<ICommandResult> Submit<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
