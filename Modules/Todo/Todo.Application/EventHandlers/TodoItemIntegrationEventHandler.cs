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
            
        }

        public async Task HandleAsync(TodoItemMarkedAsComplete @event)
        {

        }

        public async Task HandleAsync(TodoItemMarkedAsUnComplete @event)
        {

        }

        public async Task HandleAsync(TodoItemTitleUpdated @event)
        {

        }
    }
}