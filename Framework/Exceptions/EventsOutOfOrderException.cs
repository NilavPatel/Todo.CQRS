using System;

namespace Framework.Exceptions
{
    public class EventsOutOfOrderException : System.Exception
    {
        public EventsOutOfOrderException(Guid id)
            : base($"EventStore gave events for aggregate {id} out of order")
        { }
    }
}
