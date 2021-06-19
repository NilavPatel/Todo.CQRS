using System.Threading.Tasks;
using Todo.Contracts.Commands;
using Todo.Domain.DomainModels;
using Framework.Commands;
using Framework.CommandBus;
using Framework.Utils;
using Framework.Session;
using System.Net;

namespace Todo.Application.CommandHanders
{
    public class TodoItemCommandHandler :
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<MarkTodoItemAsComplete>,
        ICommandHandler<MarkTodoItemAsUnComplete>,
        ICommandHandler<UpdateTodoItemTitle>
    {
        private ISessionRepository _sessionRepository;
        public TodoItemCommandHandler(ISessionRepository sessionRepository)
        {
            this._sessionRepository = sessionRepository;
        }

        public async Task<ICommandResult> HandleAsync(CreateTodoItem createTodo)
        {
            var todoItem = new TodoItem(CombGuid.NewGuid(), createTodo.Title, createTodo.IsComplete);
            await this._sessionRepository.AddAsync(todoItem);
            await this._sessionRepository.CommitAsync();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> HandleAsync(MarkTodoItemAsComplete command)
        {
            var todoItem = await this._sessionRepository.GetAsync<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsComplete();
            await this._sessionRepository.CommitAsync();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> HandleAsync(MarkTodoItemAsUnComplete command)
        {
            var todoItem = await this._sessionRepository.GetAsync<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsUnComplete();
            await this._sessionRepository.CommitAsync();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> HandleAsync(UpdateTodoItemTitle command)
        {
            var todoItem = await this._sessionRepository.GetAsync<TodoItem>(command.Id, command.Version);
            todoItem.UpdateTodoItemTitle(command.Title);
            await this._sessionRepository.CommitAsync();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }
    }
}