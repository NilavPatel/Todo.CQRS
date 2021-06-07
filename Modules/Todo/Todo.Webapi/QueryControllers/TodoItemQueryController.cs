using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.ReadModels;
using Framework.Repository;

namespace Todo.Webapi.Controllers
{
    [Route("api/TodoItem")]
    [ApiController]
    public class TodoItemQueryController : Controller
    {
        private IReadRepository<TodoContext, TodoItem> _todoItemRepository;

        public TodoItemQueryController(IReadRepository<TodoContext, TodoItem> todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        [Route("GetTodoItems")]
        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            return await this._todoItemRepository.ListAllAsync();
        }

        [Route("GetCompletedTodoItems")]
        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetCompletedTodoItems()
        {
            var spec = new BaseSpecification<TodoItem>(t => t.IsComplete == true);
            return await this._todoItemRepository.ListAsync(spec);
        }
    }
}