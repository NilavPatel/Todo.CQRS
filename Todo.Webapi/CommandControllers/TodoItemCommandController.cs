using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Contracts.Commands;
using Todo.Framework.Core;
using Todo.Framework.Core.CommandBus;

namespace Todo.Webapi.CommandControllers
{
    [Route("api/TodoItem")]
    [ApiController]
    public class TodoItemCommandController : BaseController
    {
        public TodoItemCommandController(ICommandBus bus) : base(bus)
        { }

        [Route("CreateTodoItem")]
        [HttpPost]
        public async Task<ICommandResult> CreateTodoItem(CreateTodoItem createTodoItem)
        {
            return await _bus.Submit(createTodoItem);
        }

        [Route("MarkTodoItemAsComplete")]
        [HttpPost]
        public async Task<ICommandResult> MarkTodoItemAsComplete(MarkTodoItemAsComplete markTodoItemAsComplete)
        {
            return await _bus.Submit(markTodoItemAsComplete);
        }

        [Route("MarkTodoItemAsUnComplete")]
        [HttpPost]
        public async Task<ICommandResult> MarkTodoItemAsUnComplete(MarkTodoItemAsUnComplete markTodoItemAsUnComplete)
        {
            return await _bus.Submit(markTodoItemAsUnComplete);
        }

        [Route("UpdateTodoItemTitle")]
        [HttpPost]
        public async Task<ICommandResult> UpdateTodoItemTitle(UpdateTodoItemTitle updateTodoItemTitle)
        {
            return await _bus.Submit(updateTodoItemTitle);
        }
    }
}