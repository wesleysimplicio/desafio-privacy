using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Application.Services;
using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using MongoDB.Bson;
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
        public async Task GetOrderByIdAsync_ShouldReturnOrderDto()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            var lis = new List<OrderItem>
                {
                    new OrderItem("Product1", 2, 10.0m)
                };
            var order = new Order("John Doe",true, lis, ObjectId.Parse(orderId), DateTime.Now);


            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            Assert.Equal("John Doe", result.CustomerName);
            Assert.True(result.Status); // Status agora é bool
            Assert.Single(result.Items);
            Assert.Equal("Product1", result.Items.First().ProductName);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnListOfOrderDtos()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            var lis = new List<OrderItem>
                {
                    new OrderItem("Product1", 2, 10.0m)
                };
            var orders = new List<Order>
            {
                 new Order("John Doe",true, lis, ObjectId.Parse(orderId), DateTime.Now)
            };

            _mockOrderRepository.Setup(repo => repo.GetOrderAsync()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetOrderAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().CustomerName);
            Assert.True(result.First().Status); // Status agora é bool
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldSendMessageToQueue()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                CustomerName = "John Doe",
                Status = true, // Status agora é bool
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductName = "Product1", Quantity = 2, Price = 10.0m }
                }
            };

            _mockMessageProducer.Setup(producer => producer.SendMessageAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            await _orderService.CreateOrderAsync(orderDto);

            // Assert
            _mockMessageProducer.Verify(producer => producer.SendMessageAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            var lis = new List<OrderItem>
                {
                    new OrderItem("Product1", 2, 10.0m)
                };

            var existingOrder = new Order("John Doe", true, lis, ObjectId.Parse(orderId), DateTime.Now);

            var updatedOrderDto = new OrderDto
            {
                Id = orderId,
                CustomerName = "John Doe Updated",
                Status = false, // Status agora é bool
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductName = "Product1", Quantity = 3, Price = 15.0m }
                }
            };

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(existingOrder);
            _mockOrderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            await _orderService.UpdateOrderAsync(updatedOrderDto);

            // Assert
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            _mockOrderRepository.Setup(repo => repo.DeleteAsync(orderId)).Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteOrderAsync(orderId);

            // Assert
            _mockOrderRepository.Verify(repo => repo.DeleteAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowException_WhenOrderNotFound()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId().ToString();
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync((Order)null);

            var updatedOrderDto = new OrderDto
            {
                Id = orderId,
                CustomerName = "John Doe Updated",
                Status = false, // Status agora é bool
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductName = "Product1", Quantity = 3, Price = 15.0m }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateOrderAsync(updatedOrderDto));
        }
    }
}