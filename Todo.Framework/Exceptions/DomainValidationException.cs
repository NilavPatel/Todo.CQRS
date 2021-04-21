using System;

namespace Todo.Framework.Exceptions
{
    public class DomainValidationException : System.Exception
    {
        public DomainValidationException(string message)
            : base(message)
        { }
    }
}
