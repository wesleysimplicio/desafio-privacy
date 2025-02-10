﻿using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageProducer _messageProducer;

        public OrderService(IOrderRepository orderRepository, IMessageProducer messageProducer)
        {
            _orderRepository = orderRepository;
            _messageProducer = messageProducer;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrderAsync()
        {
            return await _orderRepository.GetOrderAsync();
        }

        public async Task CreateOrderAsync(CreateOrderRequest request)
        {
            var orderItems = request.Items.Select(item => new OrderItem(item.ProductName, item.Quantity, item.Price)).ToList();
            var order = new Order(request.CustomerName, request.Status, orderItems);
            //await _orderRepository.AddAsync(order);

            // Enviar o pedido para a fila do RabbitMQ
            await _messageProducer.SendMessageAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            //await _orderRepository.UpdateAsync(order);

            // Enviar o pedido para a fila do RabbitMQ
            await _messageProducer.SendMessageAsync(order);
        }

        public async Task DeleteOrderAsync(string id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
