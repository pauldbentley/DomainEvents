namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Pdb.DomainEvents.Abstractions;
    using Pdb.DomainEvents.EventDispatching.ServiceProvider;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services, params Type[] handlerMarkerAssemblyTypes)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Avoid double registration
            if (services.Any(sd => sd.ServiceType == typeof(IDomainEventDispatcher)))
            {
                return services;
            }

            services.AddScoped<IDomainEventDispatcher, ServiceProviderDomainEventDispatcher>();

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
                    services.AddTransient(interfaceType, handler);
                }
            }

            return services;
        }
    }
}
