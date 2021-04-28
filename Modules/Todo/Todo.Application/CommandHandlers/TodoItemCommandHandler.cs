using System.Threading.Tasks;
using Todo.Contracts.Commands;
using Todo.Domain.DomainModels;
using Framework.Command;
using Framework.CommandBus;
using Framework.Generators;
using Framework.UnitOfWork;
using System.Net;

namespace Todo.Application.CommandHanders
{
    public class TodoItemCommandHandler :
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<MarkTodoItemAsComplete>,
        ICommandHandler<MarkTodoItemAsUnComplete>,
        ICommandHandler<UpdateTodoItemTitle>
    {
        private IUowRepository _uowRepository;
        public TodoItemCommandHandler(IUowRepository uowRepository)
        {
            this._uowRepository = uowRepository;
        }

        public async Task<ICommandResult> Handle(CreateTodoItem createTodo)
        {
            var todoItem = new TodoItem(CombGuid.NewGuid(), createTodo.Title, createTodo.IsComplete);
            await this._uowRepository.Add(todoItem);
            await this._uowRepository.Commit();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> Handle(MarkTodoItemAsComplete command)
        {
            var todoItem = await this._uowRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsComplete();
            await this._uowRepository.Commit();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> Handle(MarkTodoItemAsUnComplete command)
        {
            var todoItem = await this._uowRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsUnComplete();
            await this._uowRepository.Commit();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }

        public async Task<ICommandResult> Handle(UpdateTodoItemTitle command)
        {
            var todoItem = await this._uowRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.UpdateTodoItemTitle(command.Title);
            await this._uowRepository.Commit();
            return new CommandResult(HttpStatusCode.OK, new { AggregateId = todoItem.Id, AggregateVersion = todoItem.Version });
        }
    }
}