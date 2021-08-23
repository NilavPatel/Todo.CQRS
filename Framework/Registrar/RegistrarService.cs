using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Framework.Aggregate;
using Framework.Commands;
using Framework.CommandBus;
using Framework.Events;
using Framework.Repository;
using Framework.Session;
using Framework.EventBus;
using Framework.Snapshotting;
using Framework.EventStore;
using Framework.BackgroundProcessor;
using Framework.CheckpointStore;
using EventStore.ClientAPI;

namespace Framework.Registrar
{
    public static class RegistrarService
    {
        public static void AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var eventStoreConnection = EventStoreConnection.Create(
                connectionString: configuration.GetValue<string>("EventStore:ConnectionString"),
                builder: ConnectionSettings.Create().KeepReconnecting(),
                connectionName: configuration.GetValue<string>("EventStore:ConnectionName"));

            eventStoreConnection.ConnectAsync().GetAwaiter().GetResult();

            services.AddSingleton(eventStoreConnection);
        }

        public static void RegisterFrameworkServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, DefaultCommandBus>();
            services.AddScoped<IDomainEventBus, DomainEventBus>();
            services.AddScoped<IIntegrationEventBus, IntegrationEventBus>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ISnapshotRepository, SnapshotRepository>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IBackgroundEventProcessor, BackgroundEventProcessor>();
            services.AddScoped<ICheckpointRepository, CheckpointRepository>();
        }

        public static void RegisterCommandHandlers(this IServiceCollection services, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
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

        public static void RegisterDomainEventHandlers(this IServiceCollection services, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var handlers = assembly.GetTypes()
                         .Where(t => t.GetInterfaces()
                            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));
                foreach (var i in interfaces)
                {
                    services.AddScoped(i, handler);
                }
            }
        }
        public static void RegisterIntegrationEventHandlers(this IServiceCollection services, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var handlers = assembly.GetTypes()
                         .Where(t => t.GetInterfaces()
                         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));
                foreach (var i in interfaces)
                {
                    services.AddScoped(i, handler);
                }
            }
        }
    }
}