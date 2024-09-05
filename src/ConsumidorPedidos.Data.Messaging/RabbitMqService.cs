using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ConsumidorPedidos.Data.Messaging
{
    /// <summary>
    /// Service for managing RabbitMQ connections and channels.
    /// </summary>
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqService"/> class.
        /// Reads RabbitMQ settings from environment variables or configuration and creates a connection.
        /// </summary>
        /// <param name="configuration">Application configuration for RabbitMQ settings.</param>
        public RabbitMqService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMq:HostName"] ?? "localhost",
                UserName = configuration["RabbitMq:UserName"] ?? "guest",
                Password = configuration["RabbitMq:Password"] ?? "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare a queue (creates if it does not exist)
            _channel.QueueDeclare(queue: "queue_order",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        /// <summary>
        /// Gets the current RabbitMQ channel.
        /// </summary>
        /// <returns>The RabbitMQ channel (<see cref="IModel"/>).</returns>
        public IModel GetChannel() => _channel;

        /// <summary>
        /// Closes the RabbitMQ connection and channel.
        /// </summary>
        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
