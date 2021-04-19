using System;
using System.Threading.Tasks;
using Todo.Contracts.Commands;
using Todo.Domain.DomainModels;
using Todo.Framework.Core.Aggregate;
using Todo.Framework.Core.Command;
using Todo.Framework.Core.CommandBus;

namespace Todo.Application.CommandHanders
{
    public class TodoItemCommandHandler : AggregateHandler,
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<MarkTodoItemAsComplete>,
        ICommandHandler<MarkTodoItemAsUnComplete>,
        ICommandHandler<UpdateTodoItemTitle>
    {
        private IAggregateRepository _aggregateRepository;
        public TodoItemCommandHandler(ICommandBus bus, IAggregateRepository aggregateRepository) :
            base(bus)
        {
            this._aggregateRepository = aggregateRepository;
        }

        public async Task<ICommandResult> Handle(CreateTodoItem createTodo)
        {
            var todoItem = new TodoItem(Guid.NewGuid(), createTodo.Title, createTodo.IsComplete);
            return await HandleCommand(todoItem);
        }

        public async Task<ICommandResult> Handle(MarkTodoItemAsComplete command)
        {
            var todoItem = await this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsComplete();
            return await HandleCommand(todoItem);
        }

        public async Task<ICommandResult> Handle(MarkTodoItemAsUnComplete command)
        {
            var todoItem = await this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsUnComplete();
            return await HandleCommand(todoItem);
        }

        public async Task<ICommandResult> Handle(UpdateTodoItemTitle command)
        {
            var todoItem = await this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.UpdateTodoItemTitle(command.Title);
            return await HandleCommand(todoItem);
        }
    }
}