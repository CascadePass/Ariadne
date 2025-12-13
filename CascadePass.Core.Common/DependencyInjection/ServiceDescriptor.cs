using System;

namespace CascadePass.Core.Common.DependencyInjection
{
    /// <summary>
    /// Represents a service registration within the dependency injection container.
    /// </summary>
    /// <remarks>
    /// A <see cref="ServiceDescriptor"/> holds metadata about a service type,
    /// its implementation type, lifetime, and any cached instance.
    /// </remarks>
    public class ServiceDescriptor
    {
        /// <summary>
        /// Gets the service type being registered.
        /// </summary>
        /// <example>
        /// For a registration of <c>IRepository</c>, this property will be <c>typeof(IRepository)</c>.
        /// </example>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the implementation type used to fulfill the service.
        /// </summary>
        /// <example>
        /// For a registration of <c>IRepository</c> to <c>SqlRepository</c>,
        /// this property will be <c>typeof(SqlRepository)</c>.
        /// </example>
        public Type ImplementationType { get; }

        /// <summary>
        /// Gets the lifetime of the service (e.g., singleton or transient).
        /// </summary>
        public Lifetime Lifetime { get; }

        /// <summary>
        /// Gets or sets the cached instance of the service, if applicable.
        /// </summary>
        /// <remarks>
        /// This property is typically populated when a service is registered
        /// as a singleton or when <c>RegisterInstance</c> is used.
        /// </remarks>
        public object CachedInstance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="service">The service type being registered.</param>
        /// <param name="impl">The implementation type used to fulfill the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        public ServiceDescriptor(Type service, Type impl, Lifetime lifetime)
        {
            ServiceType = service;
            ImplementationType = impl;
            Lifetime = lifetime;
        }
    }
}
