using System;
using Todo.Contracts.Commands;
using Todo.Domain.DomainModels;
using Todo.Framework.Core.Aggregate;
using Todo.Framework.Core.Command;
using Todo.Framework.Core.CommandBus;
using Todo.Framework.Core.Repository;

namespace Todo.Domain.CommandHandlers
{
    public class TodoItemHandler : AggregateHandler,
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<MarkTodoItemAsComplete>,
        ICommandHandler<MarkTodoItemAsUnComplete>,
        ICommandHandler<UpdateTodoItemTitle>
    {
        private IAggregateRepository _aggregateRepository;
        public TodoItemHandler(ICommandBus bus, IAggregateRepository aggregateRepository) :
            base(bus)
        {
            this._aggregateRepository = aggregateRepository;
        }

        public ICommandResult Handle(CreateTodoItem createTodo)
        {
            var todoItem = new TodoItem(Guid.NewGuid(), createTodo.Title, createTodo.IsComplete);
            return HandleCommand(todoItem);
        }

        public ICommandResult Handle(MarkTodoItemAsComplete command)
        {
            var todoItem = this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsComplete();
            return HandleCommand(todoItem);
        }

        public ICommandResult Handle(MarkTodoItemAsUnComplete command)
        {
            var todoItem = this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.MarkTodoItemAsUnComplete();
            return HandleCommand(todoItem);
        }

        public ICommandResult Handle(UpdateTodoItemTitle command)
        {
            var todoItem = this._aggregateRepository.Get<TodoItem>(command.Id, command.Version);
            todoItem.UpdateTodoItemTitle(command.Title);
            return HandleCommand(todoItem);
        }
    }
}