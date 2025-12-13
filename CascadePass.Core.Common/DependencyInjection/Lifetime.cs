namespace CascadePass.Core.Common.DependencyInjection
{
    /// <summary>
    /// Specifies the lifetime of a service within the dependency injection container.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// A single instance is created and reused for all resolutions.
        /// </summary>
        Singleton,

        /// <summary>
        /// A new instance is created each time the service is resolved.
        /// </summary>
        Transient
    }

}
