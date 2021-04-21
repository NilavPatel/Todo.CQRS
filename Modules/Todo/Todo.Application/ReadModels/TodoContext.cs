using Microsoft.EntityFrameworkCore;

namespace Todo.Application.ReadModels
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
          : base(options)
        { }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}