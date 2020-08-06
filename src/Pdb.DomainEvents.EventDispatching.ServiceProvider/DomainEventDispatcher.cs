﻿namespace Pdb.DomainEvents.EventDispatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Pdb.DomainEvents.Abstractions;

    // https://gist.github.com/jbogard/54d6569e883f63afebc7
    // http://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _services;

        protected DomainEventDispatcher(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Dispatch(IDomainEvent domainEvent)
        {
            var handlers = GetHandlers(domainEvent);

            foreach (var handler in handlers)
            {
                await handler.Handle(domainEvent).ConfigureAwait(false);
            }
        }

        private IEnumerable<object> GetHandlerServices(Type serviceType) =>
            _services.GetServices(serviceType);

        private IEnumerable<Handler> GetHandlers(IDomainEvent domainEvent)
        {
            // get the type of IDomainEventHandler service to handle the event
            var serviceType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            // get all the available services for the handler type
            var handlerServices = GetHandlerServices(serviceType);

            // get the type of Handler<> to handle the event
            var handlerType = typeof(Handler<>).MakeGenericType(domainEvent.GetType());

            // create the handlers for each service
            // in order (lowest number first)
            var wrappedHandlers = handlerServices
                .Select(handler => Activator.CreateInstance(handlerType, handler))
                .Select(handler => (Handler)handler)
                .OrderBy(handler => handler.Order);

            return wrappedHandlers;
        }

        private abstract class Handler
        {
            public virtual int Order { get; }

            public abstract Task Handle(IDomainEvent domainEvent);
        }

        private class Handler<T> : Handler
            where T : IDomainEvent
        {
            private readonly IDomainEventHandler<T> _handler;

            public Handler(IDomainEventHandler<T> handler)
            {
                _handler = handler;
            }

            public override int Order => _handler.Order;

            public override Task Handle(IDomainEvent domainEvent) =>
                _handler.Handle((T)domainEvent);
        }
    }
}