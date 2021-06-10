using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Framework.EventStore;
using Framework.Events;

namespace Framework.EventBus
{
    public class DomainEventBus : IDomainEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventRepository _eventRepository;

        public DomainEventBus(IServiceProvider serviceProvider, IEventRepository eventRepository)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task PublishAsync<TEvent>(TEvent eve) where TEvent : IEvent
        {
            var type = typeof(IDomainEventHandler<>).MakeGenericType(eve.GetType());
            var subscribers = this._serviceProvider.GetServices(type);
            foreach (var subscriber in subscribers)
            {
                await ((dynamic)subscriber).HandleAsync((dynamic)eve);
            }
            await this._eventRepository.MarkEventAsSuccessAsync(eve.EventId);
        }
    }
}