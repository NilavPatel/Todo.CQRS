using System;
using Todo.Contracts.Events;
using Todo.Framework.Core.Aggregate;

namespace Todo.Domain.DomainModels
{
    public class TodoItem : Aggregate
    {
        private TodoItem()
        { }
        public TodoItem(Guid id, string title, bool isComplete)
        {
            if (title.Length == 0)
            {
                throw new Exception("Title is required");
            }
            if (title.Length > 50)
            {
                throw new Exception("Title length should not be more than 50 characters");
            }
            var @event = new TodoItemCreated
            {
                Id = id,
                Title = title,
                IsComplete = isComplete
            };
            ApplyEvent(@event);
        }

        private string Title { get; set; }
        private bool IsComplete { get; set; }

        public void MarkTodoItemAsComplete()
        {
            if (this.IsComplete)
            {
                throw new Exception("Todo item is already marked as complete");
            }
            var @event = new TodoItemMarkedAsComplete();
            ApplyEvent(@event);
        }

        public void When(TodoItemCreated @event)
        {
            this.Id = @event.Id;
            this.Title = @event.Title;
            this.IsComplete = @event.IsComplete;
        }

        public void When(TodoItemMarkedAsComplete @event)
        {
            this.IsComplete = true;
        }
    }
}