using System;

namespace Framework.Exceptions
{
    public class EventIdIncorrectException : System.Exception
    {
        public EventIdIncorrectException( Guid id, Guid aggregateId)
            : base($"Event {id} has a different Id from it's aggregates Id ({aggregateId})")
        { }
    }
}
