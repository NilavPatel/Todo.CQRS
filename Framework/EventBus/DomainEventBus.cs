using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Framework.Events;

namespace Framework.EventBus
{
    public class DomainEventBus : IDomainEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventBus(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task PublishAsync<TEvent>(TEvent eve) where TEvent : IEvent
        {
            var type = typeof(IDomainEventHandler<>).MakeGenericType(eve.GetType());
            var subscribers = this._serviceProvider.GetServices(type);
            foreach (var subscriber in subscribers)
            {
                await ((dynamic)subscriber).HandleAsync((dynamic)eve);
            }
        }
    }
}