using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Contracts.Commands;
using Framework.CommandBus;

namespace Todo.Webapi.CommandControllers
{
    [Route("api/TodoItem")]
    [ApiController]
    public class TodoItemCommandController : Controller
    {
        public ICommandBus _bus;
        public TodoItemCommandController(ICommandBus bus)
        {
            _bus = bus;
        }

        [Route("CreateTodoItem")]
        [HttpPost]
        public async Task<ICommandResult> CreateTodoItem(CreateTodoItem createTodoItem)
        {
            return await _bus.SubmitAsync(createTodoItem);
        }

        [Route("MarkTodoItemAsComplete")]
        [HttpPost]
        public async Task<ICommandResult> MarkTodoItemAsComplete(MarkTodoItemAsComplete markTodoItemAsComplete)
        {
            return await _bus.SubmitAsync(markTodoItemAsComplete);
        }

        [Route("MarkTodoItemAsUnComplete")]
        [HttpPost]
        public async Task<ICommandResult> MarkTodoItemAsUnComplete(MarkTodoItemAsUnComplete markTodoItemAsUnComplete)
        {
            return await _bus.SubmitAsync(markTodoItemAsUnComplete);
        }

        [Route("UpdateTodoItemTitle")]
        [HttpPost]
        public async Task<ICommandResult> UpdateTodoItemTitle(UpdateTodoItemTitle updateTodoItemTitle)
        {
            return await _bus.SubmitAsync(updateTodoItemTitle);
        }
    }
}