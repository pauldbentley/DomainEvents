namespace Pdb.DomainEvents.EventDispatching.ServiceProvider
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    internal class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;
        private readonly IServiceProvider _currentServiceProvider;

        public ServiceLocator(IServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocator Current => new ServiceLocator(_serviceProvider);

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return _currentServiceProvider.GetService<TService>();
        }
    }
}
