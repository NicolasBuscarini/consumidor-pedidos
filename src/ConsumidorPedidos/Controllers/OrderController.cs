using ConsumidorPedidos.Core.Service.Interface;
using ConsumidorPedidos.Model;
using ConsumidorPedidos.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsumidorPedidos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, ILogger<OrderController> logger) : BaseController(logger)
    {

        /// <summary>
        /// Retrieves orders for a specific client code with pagination.
        /// </summary>
        /// <param name="clientCode">The client code to filter orders by.</param>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <param name="pageSize">The number of orders per page. Default is 10.</param>
        /// <returns>A paginated list of orders in a <see cref="BaseResponse{T}"/> format.</returns>
        /// <response code="200">Returns a paginated list of orders for the specified client code.</response>
        /// <response code="400">If the client code is invalid.</response>
        [HttpGet("by-client")]
        [ProducesResponseType(typeof(BaseResponse<List<Order>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<Order>>), 400)]
        public async Task<IActionResult> GetOrdersByClientCode([FromQuery] int clientCode, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (clientCode <= 0)
            {
                _logger.LogWarning($"Invalid client code: {clientCode}");
                return BadRequest(new BaseResponse<List<Order>>
                {
                    Error = new ErrorResponse(400) { Message = "Client code must be a positive integer." }
                });
            }

            _logger.LogInformation($"Fetching orders for client code: {clientCode} with pagination");

            try
            {
                var (orders, meta) = await orderService.GetOrdersByClientCode(clientCode, pageNumber, pageSize);
                if (orders != null)
                {
                    _logger.LogInformation($"Successfully fetched orders for client code: {clientCode}");
                    return CreateResponse(orders, meta, nameof(GetOrdersByClientCode), new { clientCode, pageNumber, pageSize });
                }

                return HandleNotFound<Order>($"No orders found for client code: {clientCode}");
            }
            catch (Exception ex)
            {
                return HandleServerError<Order>($"An error occurred while fetching orders for client code: {clientCode}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all orders with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <param name="pageSize">The number of orders per page. Default is 10.</param>
        /// <returns>A paginated list of all orders in a <see cref="BaseResponse{T}"/> format.</returns>
        /// <response code="200">Returns a paginated list of all orders.</response>
        /// <response code="404">If no orders are found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<Order>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<Order>>), 404)]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Fetching all orders with pagination");

            try
            {
                var (orders, meta) = await orderService.GetAllOrder(pageNumber, pageSize);
                if (orders != null)
                {
                    _logger.LogInformation("Successfully fetched all orders");
                    return CreateResponse(orders, meta, nameof(GetAllOrders), new { pageNumber, pageSize });
                }

                return HandleNotFound<Order>("No orders found");
            }

            catch (Exception ex)
            {
                return HandleServerError<Order>($"An error occurred while fetching orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order details in a <see cref="BaseResponse{T}"/> format.</returns>
        /// <response code="200">Returns the details of the order.</response>
        /// <response code="404">If the order with the specified ID is not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<Order>), 200)]
        [ProducesResponseType(typeof(BaseResponse<Order>), 404)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            _logger.LogInformation($"Fetching order with ID: {id}");

            try
            {
                var order = await orderService.GetOrderById(id);
                var response = new BaseResponse<Order>
                {
                    Data = order,
                    Links =
                    [
                        new("self", Url.Action(nameof(GetOrderById), new { id })!, "GET")
                    ]
                };

                _logger.LogInformation($"Successfully fetched order with ID: {id}");
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Order with ID: {id} not found. {ex.Message}");
                return HandleNotFound<Order>($"Order with ID: {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching the order: {ex.Message}");
                return HandleServerError<Order>($"An error occurred while fetching the order: {ex.Message}");
            }
        }


        /// <summary>
        /// Queues a new order to be processed.
        /// </summary>
        /// <param name="order">The order to queue.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> indicating whether the order was successfully queued.</returns>
        /// <response code="201">Indicates that the order was successfully queued.</response>
        /// <response code="400">If there was a problem queueing the order.</response>
        [HttpPost("Queue")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 201)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        public async Task<IActionResult> QueueOrder(Order order)
        {
            _logger.LogInformation("Queueing a new order");

            try
            {
                var queuedOrder = await orderService.QueueOrder(order);
                if (queuedOrder)
                {
                    _logger.LogInformation("Order successfully queued");

                    var response = new BaseResponse<bool>
                    {
                        Data = queuedOrder,
                        Links =
                        [
                            new("self", Url.Action(nameof(QueueOrder), null)!, "POST")
                        ]
                    };

                    return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, response);
                }

                return BadRequest(new BaseResponse<bool> { Error = new ErrorResponse(400) { Message = "Failed to queue the order" } });
            }
            catch (Exception ex)
            {
                return HandleServerError($"An error occurred while queueing the order: {ex.Message}");
            }
        }
    }
}
