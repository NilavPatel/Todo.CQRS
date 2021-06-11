using System;
using System.Threading.Tasks;
using Framework.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.EventBus
{
    public class IntegrationEventBus : IIntegrationEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public IntegrationEventBus(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task PublishAsync<TEvent>(TEvent eve) where TEvent : IEvent
        {
            var type = typeof(IIntegrationEventHandler<>).MakeGenericType(eve.GetType());
            var subscribers = this._serviceProvider.GetServices(type);
            foreach (var subscriber in subscribers)
            {
                await ((dynamic)subscriber).HandleAsync((dynamic)eve);
            }
        }
    }
}