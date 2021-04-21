using System;

namespace Framework.Exceptions
{
    public class AggregateVersionIncorrectException : System.Exception
    {
        public AggregateVersionIncorrectException()
            : base($"Aggregate version should not be less then or equal to 0")
        { }
    }
}
