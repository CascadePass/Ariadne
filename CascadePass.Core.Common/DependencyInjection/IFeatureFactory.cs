using System;

namespace CascadePass.Core.Common.DependencyInjection
{
    /// <summary>
    /// Defines a factory interface for resolving service instances
    /// from the dependency injection container.
    /// </summary>
    public interface IFeatureFactory
    {
        /// <summary>
        /// Resolves an instance of the specified generic service type.
        /// </summary>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T"/> if registered;
        /// otherwise, an exception is thrown.
        /// </returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves an instance of the specified service type.
        /// </summary>
        /// <param name="type">The service type to resolve.</param>
        /// <returns>
        /// An object instance of the requested <paramref name="type"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the requested type has not been registered.
        /// </exception>
        object Resolve(Type type, params object[] externalParameters);
    }

}
