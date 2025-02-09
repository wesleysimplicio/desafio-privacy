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
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageProducer _messageProducer;

        public OrderService(IOrderRepository orderRepository, IMessageProducer messageProducer)
        {
            _orderRepository = orderRepository;
            _messageProducer = messageProducer;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task CreateOrderAsync(string customerName, bool status, List<OrderItem> items)
        {
            var order = new Order(customerName, status, items);
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

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
