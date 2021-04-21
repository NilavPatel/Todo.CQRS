using System.Threading.Tasks;

namespace Todo.Framework.Event
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}