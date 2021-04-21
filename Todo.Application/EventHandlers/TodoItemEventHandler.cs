using System.Threading.Tasks;
using Todo.Application.ReadModels;
using Todo.Contracts.Events;
using Todo.Framework.Event;
using Todo.Framework.Repository;

namespace Todo.Application.EventHanders
{
    public class TodoItemEventHandler :
        IEventHandler<TodoItemCreated>,
        IEventHandler<TodoItemMarkedAsComplete>,
        IEventHandler<TodoItemMarkedAsUnComplete>,
        IEventHandler<TodoItemTitleUpdated>
    {
        private IBaseRepository<TodoContext, TodoItem> _todoItemRepository;

        public TodoItemEventHandler(IBaseRepository<TodoContext, TodoItem> todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        public async Task Handle(TodoItemCreated @event)
        {
            var todoItem = new TodoItem
            {
                Id = @event.SourceId,
                Version = @event.Version,
                Title = @event.Title,
                IsComplete = @event.IsComplete
            };
            await this._todoItemRepository.AddAsync(todoItem);
        }

        public async Task Handle(TodoItemMarkedAsComplete @event)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(@event.SourceId);
            todoItem.IsComplete = true;
            todoItem.Version = @event.Version;
            await this._todoItemRepository.UpdateAsync(todoItem);
        }

        public async Task Handle(TodoItemMarkedAsUnComplete @event)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(@event.SourceId);
            todoItem.IsComplete = false;
            todoItem.Version = @event.Version;
            await this._todoItemRepository.UpdateAsync(todoItem);
        }

        public async Task Handle(TodoItemTitleUpdated @event)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(@event.SourceId);
            todoItem.Title = @event.Title;
            todoItem.Version = @event.Version;
            await this._todoItemRepository.UpdateAsync(todoItem);
        }
    }
}