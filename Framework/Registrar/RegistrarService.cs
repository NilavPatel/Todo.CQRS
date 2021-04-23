using System;
using System.Linq;
using System.Reflection;
using Framework.Aggregate;
using Framework.Command;
using Framework.CommandBus;
using Framework.Event;
using Framework.EventStore;
using Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Registrar
{
    public class RegistrarService
    {
        private readonly IServiceCollection _services;

        public RegistrarService(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            RegisterRequiredServices();
        }

        private void RegisterRequiredServices()
        {
            _services.AddScoped<ICommandBus, DefaultCommandBus>();
            _services.AddScoped<IEventBus, DefaultEventBus>();
            _services.AddScoped<IAggregateRepository, AggregateRepository>();
            _services.AddScoped(serviceType: typeof(IBaseRepository<,>), implementationType: typeof(BaseRepository<,>));
        }

        public void AddEventStoreDbContext(string connectionString)
        {
            _services.AddDbContext<EventStoreContext>(options => options.UseSqlServer(connectionString));
        }

        public void RegisterHandlers(Assembly assembly)
        {
            RegisterCommandHandlers(assembly);
            RegisterEventHandlers(assembly);
        }

        private void RegisterCommandHandlers(Assembly assembly)
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
                    _services.AddScoped(i, handler);
                }
            }
        }

        private void RegisterEventHandlers(Assembly assembly)
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
                    _services.AddScoped(i, handler);
                }
            }
        }
    }
}