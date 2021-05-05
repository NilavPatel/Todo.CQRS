using System.Threading.Tasks;

namespace Framework.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
