using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Framework.EventStore;
using Framework.Exceptions;
using Framework.Events;

namespace Framework.EventBus
{
    public class IntegrationEventBus : IIntegrationEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventRepository _eventRepository;

        public IntegrationEventBus(IServiceProvider serviceProvider, IEventRepository eventRepository)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task PublishAsync<TEvent>(TEvent eve) where TEvent : IEvent
        {
            var type = typeof(IIntegrationEventHandler<>).MakeGenericType(eve.GetType());
            var subscribers = this._serviceProvider.GetServices(type);
            if (subscribers == null || subscribers.Count() == 0)
            {
                throw new EventHandlerNotFoundException(typeof(TEvent));
            }
            foreach (var subscriber in subscribers)
            {
                await ((dynamic)subscriber).HandleAsync((dynamic)eve);
            }
            await this._eventRepository.MarkEventAsSuccessAsync(eve.EventId);
        }
    }
}