using System.Threading.Tasks;
using Framework.CommandBus;

namespace Framework.Command
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<ICommandResult> Handle(TCommand command);
    }
}
