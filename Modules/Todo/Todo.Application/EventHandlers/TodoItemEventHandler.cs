using System.Threading.Tasks;
using Todo.Application.ReadModels;
using Todo.Contracts.Events;
using Framework.Events;
using Framework.Repository;

namespace Todo.Application.EventHanders
{
    public class TodoItemEventHandler :
        IDomainEventHandler<TodoItemCreated>,
        IDomainEventHandler<TodoItemMarkedAsComplete>,
        IDomainEventHandler<TodoItemMarkedAsUnComplete>,
        IDomainEventHandler<TodoItemTitleUpdated>
    {
        private IUnitOfWork<TodoContext> _unitOfWork;

        public TodoItemEventHandler(IUnitOfWork<TodoContext> unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(TodoItemCreated @event)
        {
            var todoItem = new TodoItem
            {
                Id = @event.SourceId,
                Version = @event.Version,
                Title = @event.Title,
                IsComplete = @event.IsComplete
            };

            await this._unitOfWork.Repository<TodoItem>().AddAsync(todoItem);
            await this._unitOfWork.SaveChangesAsync();
        }

        public async Task HandleAsync(TodoItemMarkedAsComplete @event)
        {
            var todoItem = await this._unitOfWork.Repository<TodoItem>().GetByIdAsync(@event.SourceId);
            todoItem.IsComplete = true;
            todoItem.Version = @event.Version;
            this._unitOfWork.Repository<TodoItem>().Update(todoItem);
            await this._unitOfWork.SaveChangesAsync();
        }

        public async Task HandleAsync(TodoItemMarkedAsUnComplete @event)
        {
            var todoItem = await this._unitOfWork.Repository<TodoItem>().GetByIdAsync(@event.SourceId);
            todoItem.IsComplete = false;
            todoItem.Version = @event.Version;
            this._unitOfWork.Repository<TodoItem>().Update(todoItem);
            await this._unitOfWork.SaveChangesAsync();
        }

        public async Task HandleAsync(TodoItemTitleUpdated @event)
        {
            var todoItem = await this._unitOfWork.Repository<TodoItem>().GetByIdAsync(@event.SourceId);
            todoItem.Title = @event.Title;
            todoItem.Version = @event.Version;
            this._unitOfWork.Repository<TodoItem>().Update(todoItem);
            await this._unitOfWork.SaveChangesAsync();
        }
    }
}