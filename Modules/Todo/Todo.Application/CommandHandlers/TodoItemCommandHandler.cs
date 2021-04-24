using System;
using System.Threading.Tasks;
using Todo.Contracts.Commands;
using Todo.Domain.DomainModels;
using Framework.Aggregate;
using Framework.Command;
using Framework.CommandBus;
using Framework.Event;
using Framework.Generators;

namespace Todo.Application.CommandHanders
{
    public class TodoItemCommandHandler : AggregateHandler,
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<MarkTodoItemAsComplete>,
        ICommandHandler<MarkTodoItemAsUnComplete>,
        ICommandHandler<UpdateTodoItemTitle>
    {
        private IAggregateRepository _aggregateRepository;
        public TodoItemCommandHandler(IEventBus bus, IAggregateRepository aggregateRepository) :
            base(bus, aggregateRepository)
        {
            this._aggregateRepository = aggregateRepository;
        }

        public async Task<ICommandResult> Handle(CreateTodoItem createTodo)
        {
            var todoItem = new TodoItem(CombGuid.NewGuid(), createTodo.Title, createTodo.IsComplete);
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