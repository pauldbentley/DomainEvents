namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Pdb.DomainEvents;
    using Pdb.DomainEvents.EventDispatching.ServiceProvider;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services, params Type[] handlerMarkerAssemblyTypes)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IDomainEventDispatcher, ServiceProviderDomainEventDispatcher>();

            var assemblies = handlerMarkerAssemblyTypes.Select(e => e.Assembly);
            var handlers = assemblies
                .SelectMany(e => e.GetTypes())
                .Where(t => t
                    .GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IDomainEventHandler<>))));

            foreach (var handler in handlers)
            {
                foreach (var interfaceType in handler.GetInterfaces())
                {
                    services.TryAddTransient(interfaceType, handler);
                }
            }

            return services;
        }

        public static IApplicationBuilder UseDomainEventDispatcher(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            ServiceLocator.SetLocatorProvider(app.ApplicationServices);
            DomainEvent.Dispatcher = () => ServiceLocator.Current.GetInstance<IDomainEventDispatcher>();

            return app;
        }
    }
}
