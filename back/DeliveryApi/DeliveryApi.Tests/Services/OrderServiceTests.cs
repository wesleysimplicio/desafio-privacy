using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Application.Services;
using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApi.Tests.Application.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMessageProducer> _messageProducerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _messageProducerMock = new Mock<IMessageProducer>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _messageProducerMock.Object);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid().ToString();
            var expectedOrder = new Order("Customer Name", true, new List<OrderItem>());
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder, result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid().ToString();
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldSendMessageToQueue()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                CustomerName = "Customer Name",
                Status = true,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductName = "Product 1", Quantity = 1, Price = 10.0m }
                }
            };

            // Act
            await _orderService.CreateOrderAsync(request);

            // Assert
            _messageProducerMock.Verify(producer => producer.SendMessageAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldSendMessageToQueue()
        {
            // Arrange
            var order = new Order("Customer Name", true, new List<OrderItem>());

            // Act
            await _orderService.UpdateOrderAsync(order);

            // Assert
            _messageProducerMock.Verify(producer => producer.SendMessageAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var orderId = Guid.NewGuid().ToString();

            // Act
            await _orderService.DeleteOrderAsync(orderId);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.DeleteAsync(orderId), Times.Once);
        }
    }
}