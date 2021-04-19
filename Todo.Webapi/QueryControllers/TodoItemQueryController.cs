using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.ReadModels;
using Todo.Framework.Core.Repository;

namespace Todo.Webapi.Controllers
{
    [Route("api/TodoItem")]
    [ApiController]
    public class TodoItemQueryController : Controller
    {
        private IBaseRepository<TodoContext, TodoItem> _todoItemRepository;

        public TodoItemQueryController(IBaseRepository<TodoContext, TodoItem> todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        [Route("GetTodoItems")]
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems()
        {
            return this._todoItemRepository.ListAll();
        }

        [Route("GetCompletedTodoItems")]
        [HttpGet]
        public IEnumerable<TodoItem> GetCompletedTodoItems()
        {
            var spec = new BaseSpecification<TodoItem>(t => t.IsComplete == true);
            return this._todoItemRepository.List(spec);
        }
    }
}