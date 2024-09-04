
namespace ConsumidorPedidos.Config.Ioc
{
    /// <summary>
    /// IoC configuration class for repository services.
    /// </summary>
    public static class RepositoryIoc
    {
        /// <summary>
        /// Configures IoC container for repository services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureRepositoryIoc(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
        }
    }
}
