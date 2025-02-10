using DeliveryApi.Application.DTOs;
using DeliveryApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryApi.Application.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Obtém um pedido pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do pedido.</param>
        /// <returns>O pedido correspondente ao ID fornecido.</returns>
        Task<OrderDto> GetOrderByIdAsync(string id);

        /// <summary>
        /// Obtém todos os pedidos.
        /// </summary>
        /// <returns>Uma lista de todos os pedidos.</returns>
        Task<IEnumerable<OrderDto>> GetOrderAsync();

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="request">Os dados necessários para criar um pedido.</param>
        Task CreateOrderAsync(OrderDto request);

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="order">O pedido a ser atualizado.</param>
        Task UpdateOrderAsync(OrderDto order);

        /// <summary>
        /// Deleta um pedido pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do pedido a ser deletado.</param>
        Task DeleteOrderAsync(string id);
    }
}