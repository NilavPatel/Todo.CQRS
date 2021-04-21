using System;

namespace Framework.Event
{
    public class EventHandlerNotFoundException : Exception
    {
        public EventHandlerNotFoundException(Type eventPublisherType)
        : base(string.Format("Event handler not found for event {0}.", eventPublisherType.Name))
        { }
    }
}