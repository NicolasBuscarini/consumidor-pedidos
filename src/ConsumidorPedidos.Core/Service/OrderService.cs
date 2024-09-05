using ConsumidorPedidos.Core.Repository.Interface;
using ConsumidorPedidos.Core.Service.Interface;
using ConsumidorPedidos.Model;
using ConsumidorPedidos.Model.Response;
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
        public async Task<(List<Order> Orders, MetaData Meta)> GetAllOrder(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await repository.ListAll().CountAsync();
            var orders = await repository.ListAll(pageNumber, pageSize).ToListAsync();

            var meta = new MetaData(
                totalItems: totalItems,
                itemsPerPage: pageSize,
                currentPage: pageNumber,
                totalPages: totalItems / pageSize
            );

            return (Orders: orders, Meta: meta);
        }

        /// <summary>
        /// Retrieves a paginated list of orders filtered by the client code.
        /// </summary>
        /// <param name="clientCode">The client code to filter orders by.</param>
        /// <param name="pageNumber">The page number for pagination (1-based index).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A tuple containing a list of filtered orders and pagination metadata.</returns>
        public async Task<(List<Order> Orders, MetaData Meta)> GetOrdersByClientCode(int clientCode, int pageNumber, int pageSize)
        {
            // Create a query to filter orders by client code.
            var query = repository.ListAll()
                                  .Where(o => o.ClientCode == clientCode);

            // Get the total count of items that match the filter.
            var totalItems = await query.CountAsync();

            // Retrieve the paginated list of orders.
            var orders = await query
                                 .Skip((pageNumber - 1) * pageSize)  // Skip items based on the current page.
                                 .Take(pageSize)  // Take the number of items specified by pageSize.
                                 .ToListAsync();

            // Create metadata for pagination.
            var meta = new MetaData(
                totalItems: totalItems,
                itemsPerPage: pageSize,
                currentPage: pageNumber,
                totalPages: totalItems / pageSize
            );

            // Return the orders and metadata.
            return (Orders: orders, Meta: meta);
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
