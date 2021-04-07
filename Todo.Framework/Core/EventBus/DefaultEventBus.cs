using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Todo.Framework.Core.Event
{
    public class DefaultEventBus : IEventBus
    {
        IServiceProvider _serviceProvider;
        public DefaultEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Publish<TEvent>(TEvent eve) where TEvent : IEvent
        {
            var type = typeof(IEventHandler<>).MakeGenericType(eve.GetType());
            var subscribers = _serviceProvider.GetServices(type);
            if (subscribers == null || subscribers.Count() == 0)
            {
                throw new EventHandlerNotFoundException(typeof(TEvent));
            }
            foreach (var subscriber in subscribers)
            {
                ((dynamic)subscriber).Handle((dynamic)eve);
            }
        }
    }
}