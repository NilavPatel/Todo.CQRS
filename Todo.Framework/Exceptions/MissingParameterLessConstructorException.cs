using System;

namespace Todo.Framework.Exceptions
{
    public class MissingParameterLessConstructorException : System.Exception
    {
        public MissingParameterLessConstructorException(Type type)
            : base($"{type.FullName} has no constructor without parameters. This can be either public or private")
        { }
    }
}
