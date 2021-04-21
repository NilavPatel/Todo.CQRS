using System;
using Framework.Repository;

namespace Todo.Application.ReadModels
{
    public class TodoItem : BaseEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}