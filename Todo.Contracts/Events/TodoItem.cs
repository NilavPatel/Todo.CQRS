
using System;
using Todo.Framework.Core.Event;

namespace Todo.Contracts.Events
{
    public class TodoItemCreated : Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }

    public class TodoItemMarkedAsComplete : Event
    { }

    public class TodoItemMarkedAsUnComplete : Event
    { }

    public class TodoItemTitleUpdated : Event
    {
        public string Title { get; set; }
    }
}