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
    public class MessageConsumer
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(IModel channel, IServiceScopeFactory scopeFactory, ILogger<MessageConsumer> logger)
        {
            _channel = channel;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void StartConsuming(string queueName)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($" [x] Received '{message}' from queue '{queueName}'");

                try
                {
                    await ProcessMessageAsync(message);
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError($"Error processing message: {jsonEx.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            _logger.LogInformation($" [*] Waiting for messages in '{queueName}'. To exit press CTRL+C");
        }

        private async Task ProcessMessageAsync(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                try
                {
                    var order = JsonConvert.DeserializeObject<Order>(message);

                    if (order == null || order.Items == null || order.Items.Count == 0)
                    {
                        throw new InvalidOperationException("Order data is invalid or missing required fields.");
                    }

                    _logger.LogInformation($" [x] Processing message: {message}");

                    await orderService.CreateOrder(order);
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError($"JSON error: {jsonEx.Message}");
                    throw;
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    _logger.LogError($"Validation error: {invalidOpEx.Message}");
                    throw;
                }
            }
        }
    }
}
