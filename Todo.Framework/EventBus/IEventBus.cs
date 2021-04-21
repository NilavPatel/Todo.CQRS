using System.Threading.Tasks;

namespace Todo.Framework.Event
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
