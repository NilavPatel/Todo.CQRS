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
        public ICommandResult CreateTodoItem(CreateTodoItem createTodoItem)
        {
            return _bus.Submit(createTodoItem);
        }

        [Route("MarkTodoItemAsComplete")]
        [HttpPost]
        public ICommandResult MarkTodoItemAsComplete(MarkTodoItemAsComplete markTodoItemAsComplete)
        {
            return _bus.Submit(markTodoItemAsComplete);
        }

        [Route("MarkTodoItemAsUnComplete")]
        [HttpPost]
        public ICommandResult MarkTodoItemAsUnComplete(MarkTodoItemAsUnComplete markTodoItemAsUnComplete)
        {
            return _bus.Submit(markTodoItemAsUnComplete);
        }

        [Route("UpdateTodoItemTitle")]
        [HttpPost]
        public ICommandResult UpdateTodoItemTitle(UpdateTodoItemTitle updateTodoItemTitle)
        {
            return _bus.Submit(updateTodoItemTitle);
        }
    }
}