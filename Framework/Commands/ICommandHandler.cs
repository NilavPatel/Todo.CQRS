using System.Threading.Tasks;
using Framework.CommandBus;

namespace Framework.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<ICommandResult> HandleAsync(TCommand command);
    }
}
