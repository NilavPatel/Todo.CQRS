using System;
using Todo.Framework.Core.Command;

namespace Todo.Contracts.Commands
{
    public class CreateTodoItem : Command
    {
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }

    public class MarkTodoItemAsComplete : Command
    {
        public Guid Id { get; set; }
    }
}