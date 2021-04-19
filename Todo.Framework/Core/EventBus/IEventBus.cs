using System.Threading.Tasks;

namespace Todo.Framework.Core.Event
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
