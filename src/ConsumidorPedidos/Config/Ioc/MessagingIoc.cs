using ConsumidorPedidos.Data.Messaging;
using ConsumidorPedidos.Core.Consumer;
using ConsumidorPedidos.Core.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsumidorPedidos.Config.Ioc
{
    /// <summary>
    /// IoC configuration class for Message Consumer.
    /// </summary>
    public static class MessagingIoc
    {
        /// <summary>
        /// Configures IoC container for messaging services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureMessagingIoc(this IServiceCollection services)
        {
            // Register RabbitMQ services
            services.AddSingleton<RabbitMqService>();

            // Register MessagePublisher as Singleton
            services.AddSingleton(sp =>
            {
                var rabbitMqService = sp.GetRequiredService<RabbitMqService>();
                return new MessagePublisher(rabbitMqService.GetChannel());
            });

            // Register MessageConsumer as Singleton, if appropriate
            services.AddSingleton(sp =>
            {
                var rabbitMqService = sp.GetRequiredService<RabbitMqService>();
                var logger = sp.GetRequiredService<ILogger<MessageConsumer>>();
                return new MessageConsumer(rabbitMqService.GetChannel(), sp.GetRequiredService<IServiceScopeFactory>(), logger);
            });

            services.AddScoped<ConsumerStarter>();
        }
    }
}
