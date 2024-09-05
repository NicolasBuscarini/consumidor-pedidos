using ConsumidorPedidos.Data.Messaging;
using ConsumidorPedidos.Data.MySql;
using Microsoft.Extensions.Logging;
using ConsumidorPedidos.Model;
using Newtonsoft.Json;
using ConsumidorPedidos.Core.Repository.Interface;

namespace ConsumidorPedidos.Core.Repository
{
    public class OrderRepository(AppDbContext context, MessagePublisher messagePublisher, ILogger<OrderRepository> logger) : GenericRepository<Order, int>(context, logger), IOrderRepository
    {
        private readonly AppDbContext _context = context;
        private readonly MessagePublisher _messagePublisher = messagePublisher;

        public Task<bool> Queue(Order order)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(order);

                // Publish the message to the RabbitMQ queue
                _messagePublisher.Publish("queue_order", messageBody);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
