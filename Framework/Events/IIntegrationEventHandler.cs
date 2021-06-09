using System.Threading.Tasks;

namespace Framework.Events
{
    public interface IIntegrationEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}