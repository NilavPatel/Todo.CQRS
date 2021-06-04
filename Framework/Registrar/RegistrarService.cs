using System.Linq;
using System.Reflection;
using Framework.Aggregate;
using Framework.Commands;
using Framework.CommandBus;
using Framework.Events;
using Framework.EventStore;
using Framework.Repository;
using Framework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Framework.EventBus;

namespace Framework.Registrar
{
    public static class RegistrarService
    {
        public static void AddEventStoreDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EventStoreContext>(options => options.UseSqlServer(connectionString));
        }

        public static void RegisterHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            RegisterCommandHandlers(services, assembly);
            RegisterEventHandlers(services, assembly);
        }

        public static void RegisterFrameworkServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, DefaultCommandBus>();
            services.AddScoped<IEventBus, DefaultEventBus>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped<IUowRepository, UowRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ISnapshotRepository, SnapshotRepository>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        }

        private static void RegisterCommandHandlers(IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly.GetTypes()
                         .Where(t => t.GetInterfaces()
                         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                foreach (var i in interfaces)
                {
                    services.AddScoped(i, handler);
                }
            }
        }

        private static void RegisterEventHandlers(IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly.GetTypes()
                         .Where(t => t.GetInterfaces()
                         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
                foreach (var i in interfaces)
                {
                    services.AddScoped(i, handler);
                }
            }
        }
    }
}