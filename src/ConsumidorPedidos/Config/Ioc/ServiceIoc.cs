using ConsumidorPedidos.Core.Service.Interface;
using ConsumidorPedidos.Core.Service;

namespace ConsumidorPedidos.Config.Ioc
{
    /// <summary>
    /// IoC configuration class for service services.
    /// </summary>
    public static class ServiceIoc
    {
        /// <summary>
        /// Configures IoC container for service services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureServiceIoc(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
