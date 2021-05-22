using System;

namespace Framework.Exceptions
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(Type commandHandlerType)
        : base(string.Format("Command handler not found for command {0}.", commandHandlerType.Name))
        { }
    }
}