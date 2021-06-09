using System.Threading.Tasks;
using Framework.Events;

namespace Framework.EventBus
{
    public interface IDomainEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
