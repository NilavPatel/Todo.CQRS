using System.Threading.Tasks;
using Todo.Contracts.Events;
using Framework.Events;

namespace Todo.Application.EventHanders
{
    public class TodoItemIntegrationEventHandler :
        IIntegrationEventHandler<TodoItemCreated>,
        IIntegrationEventHandler<TodoItemMarkedAsComplete>,
        IIntegrationEventHandler<TodoItemMarkedAsUnComplete>,
        IIntegrationEventHandler<TodoItemTitleUpdated>
    {
        public TodoItemIntegrationEventHandler()
        {
        }

        public async Task HandleAsync(TodoItemCreated @event)
        {
            await Task.Delay(500);
        }

        public async Task HandleAsync(TodoItemMarkedAsComplete @event)
        {
            await Task.Delay(500);
        }

        public async Task HandleAsync(TodoItemMarkedAsUnComplete @event)
        {
            await Task.Delay(500);
        }

        public async Task HandleAsync(TodoItemTitleUpdated @event)
        {
            await Task.Delay(500);
        }
    }
}