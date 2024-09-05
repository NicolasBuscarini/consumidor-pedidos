using RabbitMQ.Client;
using System.Text;

namespace ConsumidorPedidos.Data.Messaging
{
    /// <summary>
    /// Publishes messages to a RabbitMQ queue.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MessagePublisher"/> class with a RabbitMQ channel.
    /// </remarks>
    /// <param name="channel">The RabbitMQ channel to use for publishing messages.</param>
    public class MessagePublisher(IModel channel)
    {

        /// <summary>
        /// Publishes a message to the specified RabbitMQ queue.
        /// </summary>
        /// <param name="routingKey">The queue name (routing key) where the message will be sent.</param>
        /// <param name="message">The message to be sent to the queue.</param>
        public void Publish(string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            // Publishes the message to the specified queue
            channel.BasicPublish(exchange: "",
                                  routingKey: routingKey,
                                  basicProperties: null,
                                  body: body);

            // Log the message sent
            Console.WriteLine($" [x] Sent '{message}' to queue '{routingKey}'");
        }
    }
}
