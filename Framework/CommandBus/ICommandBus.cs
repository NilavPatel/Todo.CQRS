using System.Threading.Tasks;
using Framework.Commands;

namespace Framework.CommandBus
{
    public interface ICommandBus
    {
        Task<ICommandResult> SubmitAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
