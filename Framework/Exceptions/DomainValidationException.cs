using System;

namespace Framework.Exceptions
{
    public class DomainValidationException : System.Exception
    {
        public DomainValidationException(string message)
            : base(message)
        { }
    }
}
