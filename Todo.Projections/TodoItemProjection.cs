using Todo.Contracts.Events;
using Todo.Framework.Core.Event;
using Todo.Framework.Core.Repository;
using Todo.ReadModels;

namespace Todo.Projections
{
    public class TodoItemProjection :
        IEventHandler<TodoItemCreated>,
        IEventHandler<TodoItemMarkedAsComplete>,
        IEventHandler<TodoItemMarkedAsUnComplete>,
        IEventHandler<TodoItemTitleUpdated>
    {
        private IBaseRepository<TodoContext, TodoItem> _todoItemRepository;

        public TodoItemProjection(IBaseRepository<TodoContext, TodoItem> todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        public void Handle(TodoItemCreated @event)
        {
            var todoItem = new TodoItem
            {
                Id = @event.SourceId,
                Version = @event.Version,
                Title = @event.Title,
                IsComplete = @event.IsComplete
            };
            this._todoItemRepository.Add(todoItem);
        }

        public void Handle(TodoItemMarkedAsComplete @event)
        {
            var todoItem = _todoItemRepository.GetById(@event.SourceId);
            todoItem.IsComplete = true;
            todoItem.Version = @event.Version;
            this._todoItemRepository.Update(todoItem);
        }

        public void Handle(TodoItemMarkedAsUnComplete @event)
        {
            var todoItem = _todoItemRepository.GetById(@event.SourceId);
            todoItem.IsComplete = false;
            todoItem.Version = @event.Version;
            this._todoItemRepository.Update(todoItem);
        }

        public void Handle(TodoItemTitleUpdated @event)
        {
            var todoItem = _todoItemRepository.GetById(@event.SourceId);
            todoItem.Title = @event.Title;
            todoItem.Version = @event.Version;
            this._todoItemRepository.Update(todoItem);
        }
    }
}