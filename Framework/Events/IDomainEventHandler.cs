using System.Threading.Tasks;

namespace Framework.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}