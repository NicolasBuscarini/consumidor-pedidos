using ConsumidorPedidos.Model.Response;
using ConsumidorPedidos.Model;
using Microsoft.Extensions.Logging;
using Moq;
using ConsumidorPedidos.Controllers;
using ConsumidorPedidos.Core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ConsumidorPedidos.Tests.Controller
{
    public class OrderControllerTests
    {
        protected readonly OrderController _orderController;
        protected readonly Mock<IOrderService> _orderServiceMock;
        protected readonly Mock<ILogger<OrderController>> _mockLogger;

        public OrderControllerTests() : base()
        {
            // Controller
            _orderServiceMock = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<OrderController>>();
            _orderController = new OrderController(_orderServiceMock.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetOrdersByClientCode_ShouldReturnServerError_WhenExceptionOccurs()
        {
            // Arrange
            var clientCode = 123;
            var pageNumber = 1;
            var pageSize = 10;

            _orderServiceMock.Setup(s => s.GetOrdersByClientCode(clientCode, pageNumber, pageSize))
                             .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _orderController.GetOrdersByClientCode(clientCode, pageNumber, pageSize);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<BaseResponse<Order>>(internalServerErrorResult.Value);
            Assert.Equal("An error occurred while fetching orders for client code: 123: Some error", response.Error.Message);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnServerError_WhenExceptionOccurs()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            _orderServiceMock.Setup(s => s.GetAllOrder(pageNumber, pageSize))
                             .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _orderController.GetAllOrders(pageNumber, pageSize);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<BaseResponse<Order>>(internalServerErrorResult.Value);
            Assert.Equal("An error occurred while fetching orders: Some error", response.Error.Message);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnServerError_WhenExceptionOccurs()
        {
            // Arrange
            var orderId = 1;

            _orderServiceMock.Setup(s => s.GetOrderById(orderId))
                             .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _orderController.GetOrderById(orderId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<BaseResponse<Order>>(internalServerErrorResult.Value);
            Assert.Equal($"An error occurred while fetching the order: Some error", response.Error.Message);
        }

        [Fact]
        public async Task QueueOrder_ShouldReturnBadRequest_WhenOrderQueueingFails()
        {
            // Arrange
            var order = OrderTestHelper.Order;

            _orderServiceMock.Setup(s => s.QueueOrder(order))
                             .ReturnsAsync(false);

            // Act
            var result = await _orderController.QueueOrder(order);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BaseResponse<bool>>(badRequestResult.Value);
            Assert.False(response.Data);
            Assert.Equal("Failed to queue the order", response.Error!.Message);
        }

        [Fact]
        public async Task QueueOrder_ShouldReturnServerError_WhenExceptionOccurs()
        {
            // Arrange
            var order = OrderTestHelper.Order;

            _orderServiceMock.Setup(s => s.QueueOrder(order))
                             .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _orderController.QueueOrder(order);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<BaseResponse<bool>>(internalServerErrorResult.Value);
            Assert.Equal("An error occurred while queueing the order: Some error", response.Error!.Message);
        }
    }
}
