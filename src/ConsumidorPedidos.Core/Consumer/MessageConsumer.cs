using ConsumidorPedidos.Core.Service.Interface;
using ConsumidorPedidos.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsumidorPedidos.Core.Consumer
{
    public class MessageConsumer(IModel channel, IServiceScopeFactory scopeFactory, ILogger<MessageConsumer> logger)
    {
        public void StartConsuming(string queueName)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                logger.LogInformation($" [x] Received '{message}' from queue '{queueName}'");

                try
                {
                    await ProcessMessageAsync(message);
                }
                catch (JsonException jsonEx)
                {
                    logger.LogError($"Error processing message: {jsonEx.Message}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unexpected error: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            logger.LogInformation($" [*] Waiting for messages in '{queueName}'. To exit press CTRL+C");
        }

        private async Task ProcessMessageAsync(string message)
        {
            using var scope = scopeFactory.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            try
            {
                var order = JsonConvert.DeserializeObject<Order>(message);

                if (order == null || order.Items == null || order.Items.Count == 0)
                {
                    throw new InvalidOperationException("Order data is invalid or missing required fields.");
                }

                logger.LogInformation($" [x] Processing message: {message}");

                await orderService.CreateOrder(order);
            }
            catch (JsonException jsonEx)
            {
                logger.LogError($"JSON error: {jsonEx.Message}");
                throw;
            }
            catch (InvalidOperationException invalidOpEx)
            {
                logger.LogError($"Validation error: {invalidOpEx.Message}");
                throw;
            }
        }
    }
}
