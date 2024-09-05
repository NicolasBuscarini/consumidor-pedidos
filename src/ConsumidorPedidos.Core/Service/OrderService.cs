using ConsumidorPedidos.Core.Repository.Interface;
using ConsumidorPedidos.Core.Service.Interface;
using ConsumidorPedidos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsumidorPedidos.Core.Service
{
    /// <summary>
    /// Service implementation for order-related operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </remarks>
    /// <param name="repository">The product repository.</param>
    public class OrderService(IOrderRepository repository, ILogger<OrderService> logger) : IOrderService
    {

        /// <summary>
        /// Queue a new order asynchronously.
        /// </summary>
        /// <param name="order">The <see cref="Order"/></param>
        /// <returns><see cref="bool"/></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="order"/> has no items.</exception>
        public async Task<bool> QueueOrder(Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
            {
                throw new ArgumentException("Order must have at least one item.", nameof(order));
            }

            return await repository.Queue(order);
        }
        /// <summary>
        /// Creates a new order asynchronously.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to create.</param>
        /// <returns>The created <see cref="Order"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="order"/> has no items.</exception>
        public async Task<Order> CreateOrder(Order order)
        {
            try
            {
                if (order.Items == null || order.Items.Count == 0)
                {
                    throw new ArgumentException("Order must have at least one item.", nameof(order));
                }

                logger.LogInformation("Attempting to create a new order with ClientCode: {ClientCode}", order.ClientCode);
                var createdOrder = await repository.CreateAsync(order);
                logger.LogInformation("Order created successfully with Id: {OrderId}", createdOrder.Id);

                return createdOrder;
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Failed to create order due to invalid input.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the order.");
                throw;
            }
        }

        /// <summary>
        /// Deletes an order asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteOrder(int id)
        {
            var order = await repository.GetByIdAsync(id);
            if (order == null)
                return false;

            return await repository.DeleteAsync(order, order.Id);
        }

        /// <summary>
        /// Gets all orders asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="Order"/>.</returns>
        public async Task<List<Order>> GetAllOrder()
        {
            return await repository.ListAll().ToListAsync();
        }

        /// <summary>
        /// Gets a specific order by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The <see cref="Order"/> with the specified ID, or null if not found.</returns>
        public async Task<Order> GetOrderById(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing order asynchronously.
        /// </summary>
        /// <param name="order">The updated <see cref="Order"/>.</param>
        /// <returns>The updated <see cref="Order"/> if it exists; otherwise, null.</returns>
        public async Task<Order?> UpdateOrder(Order order)
        {
            var existingOrder = await repository.GetByIdAsync(order.Id);
            if (existingOrder == null)
                return null;

            existingOrder.Items = order.Items;

            await repository.UpdateAsync(existingOrder, existingOrder.ClientCode);
            return existingOrder;
        }
    }
}
