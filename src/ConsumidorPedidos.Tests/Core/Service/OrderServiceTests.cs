using ConsumidorPedidos.Core.Repository.Interface;
using ConsumidorPedidos.Core.Service;
using ConsumidorPedidos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Moq.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumidorPedidos.Tests.Utility;

namespace ConsumidorPedidos.Tests.Core.Service
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task QueueOrder_ShouldThrowArgumentException_WhenOrderHasNoItems()
        {
            // Arrange
            var order = new Order { Items = new List<Item>() };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.QueueOrder(order));
        }

        [Fact]
        public async Task QueueOrder_ShouldReturnTrue_WhenOrderIsQueuedSuccessfully()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            _repositoryMock.Setup(repo => repo.Queue(It.IsAny<Order>())).ReturnsAsync(true);

            // Act
            var result = await _orderService.QueueOrder(order);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowArgumentException_WhenOrderHasNoItems()
        {
            // Arrange
            var order = new Order { Items = new List<Item>() };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.CreateOrder(order));
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedOrder_WhenOrderIsCreatedSuccessfully()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Order>())).ReturnsAsync(order);

            // Act
            var result = await _orderService.CreateOrder(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.DeleteOrder(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnTrue_WhenOrderIsDeletedSuccessfully()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(order);
            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Order>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _orderService.DeleteOrder(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnUpdatedOrder_WhenOrderExists()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            var orders = new List<Order> { order };

            var existingOrder = OrderTestHelper.Order; 
            var updatedOrder = existingOrder;
            updatedOrder.Items[0].Quantity = 10;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingOrder);
            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Order>(), It.IsAny<int>())).ReturnsAsync(1);

            // Act
            var result = await _orderService.UpdateOrder(updatedOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            var order = OrderTestHelper.Order;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.UpdateOrder(order);

            // Assert
            Assert.Null(result);
        }
    }
}