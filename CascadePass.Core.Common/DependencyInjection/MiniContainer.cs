using System;
using System.Collections.Generic;
using System.Linq;

namespace CascadePass.Core.Common.DependencyInjection
{
    /// <summary>
    /// Provides a lightweight dependency injection container
    /// for managing service registrations and resolutions.
    /// </summary>
    /// <remarks>
    /// Designed to minimize external dependencies while encouraging
    /// clean, testable code. This container supports singleton and
    /// transient lifetimes.
    /// </remarks>
    public class MiniContainer : IFeatureFactory
    {
        private readonly Dictionary<Type, ServiceDescriptor> services = new();

        /// <summary>
        /// Registers a service type with its implementation.
        /// </summary>
        /// <typeparam name="TService">The interface or base type to register.</typeparam>
        /// <typeparam name="TImplementation">The concrete type that implements <typeparamref name="TService"/>.</typeparam>
        /// <param name="singleton">If true, the same instance is reused; otherwise a new instance is created per resolution.</param>
        /// <example>
        /// <code>
        /// container.Register<IRepository, SqlRepository>(singleton: true);
        /// </code>
        /// </example>
        public void Register<TService, TImplementation>(Lifetime lifetime)
        {
            services[typeof(TService)] =
                new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
        }

        /// <summary>
        /// Registers an existing instance of a service as a singleton
        /// within the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <param name="instance">
        /// The pre‑constructed instance to use for all resolutions of
        /// <typeparamref name="TService"/>.
        /// </param>
        /// <remarks>
        /// This method is useful when you want to supply a specific
        /// implementation or external resource (e.g., a configuration object,
        /// a database connection, or a mock for testing) rather than letting
        /// the container construct it.
        /// </remarks>
        /// <example>
        /// <code>
        /// var config = new AppConfig();
        /// container.RegisterInstance<IConfig>(config);
        /// </code>
        /// </example>
        public void RegisterInstance<TService>(TService instance)
        {
            services[typeof(TService)] = new ServiceDescriptor(
                typeof(TService),
                instance.GetType(),
                Lifetime.Singleton
            )
            {
                CachedInstance = instance
            };
        }

        /// <summary>
        /// Resolves an instance of the requested service type.
        /// </summary>
        /// <typeparam name="TService">The type of service to resolve.</typeparam>
        /// <returns>An instance of <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the requested service has not been registered.
        /// </exception>
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        /// <summary>
        /// Resolves an instance of the specified service type from the container.
        /// </summary>
        /// <param name="serviceType">
        /// The type of service to resolve. This must be a type that has been
        /// previously registered with the container.
        /// </param>
        /// <returns>
        /// An object instance of the requested <paramref name="serviceType"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the requested <paramref name="serviceType"/> has not been registered.
        /// </exception>
        /// <remarks>
        /// This overload is useful when the service type is not known at compile time,
        /// such as when resolving services dynamically or via reflection.
        /// </remarks>
        /// <example>
        /// <code>
        /// var logger = (ILogger)container.Resolve(typeof(ILogger));
        /// </code>
        /// </example>
        public object Resolve(Type serviceType)
        {
            if (!services.TryGetValue(serviceType, out var descriptor))
                throw new InvalidOperationException($"Service not registered: {serviceType}");

            // Singleton: return cached instance
            if (descriptor.Lifetime == Lifetime.Singleton && descriptor.CachedInstance != null)
                return descriptor.CachedInstance;

            // Create instance
            var implType = descriptor.ImplementationType;

            // Pick the constructor with the most parameters
            var ctor = implType.GetConstructors()
                               .OrderByDescending(c => c.GetParameters().Length)
                               .First();

            var parameters = ctor.GetParameters()
                                 .Select(p => Resolve(p.ParameterType))
                                 .ToArray();

            var instance = Activator.CreateInstance(implType, parameters);

            if (descriptor.Lifetime == Lifetime.Singleton)
                descriptor.CachedInstance = instance;

            return instance;
        }
    }
}
