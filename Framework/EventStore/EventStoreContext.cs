using Microsoft.EntityFrameworkCore;

namespace Framework.EventStore
{
    public class EventStoreContext : DbContext
    {
        public EventStoreContext(DbContextOptions<EventStoreContext> options) : base(options)
        { }

        public DbSet<EventEntity> Events { get; set; }
    }
}