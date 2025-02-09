// DeliveryApi.Tests/Services/OrderServiceTests.cs
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Application.Services;
using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApi.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMessageProducer> _mockMessageProducer;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMessageProducer = new Mock<IMessageProducer>();

            _orderService = new OrderService(_mockOrderRepository.Object, _mockMessageProducer.Object);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order("Customer1", false, new List<OrderItem> { new OrderItem("Product1", 1, 10) }, orderId);

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            _mockOrderRepository.Verify(repo => repo.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Null(result);
            _mockOrderRepository.Verify(repo => repo.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCallAddAsync_WithValidOrder()
        {
            // Arrange
            var customerName = "Customer1";
            var status = false;
            var items = new List<OrderItem> { new OrderItem("Product1", 1, 10) };

            _mockOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            await _orderService.CreateOrderAsync(customerName, status, items);

            // Assert
            _mockOrderRepository.Verify(repo => repo.AddAsync(It.Is<Order>(o => o.CustomerName == customerName && o.Status == status && o.Items.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldCallUpdateAsync_WithValidOrder()
        {
            // Arrange
            var order = new Order("Customer1", false, new List<OrderItem> { new OrderItem("Product1", 1, 10) });

            _mockOrderRepository.Setup(repo => repo.UpdateAsync(order)).Returns(Task.CompletedTask);

            // Act
            await _orderService.UpdateOrderAsync(order);

            // Assert
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldCallDeleteAsync_WithValidId()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockOrderRepository.Setup(repo => repo.DeleteAsync(orderId)).Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteOrderAsync(orderId);

            // Assert
            _mockOrderRepository.Verify(repo => repo.DeleteAsync(orderId), Times.Once);
        }
    }
}