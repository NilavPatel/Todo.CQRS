using System.Threading.Tasks;
using Framework.Events;

namespace Framework.EventBus
{
    public interface IIntegrationEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
