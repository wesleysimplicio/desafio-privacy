using DeliveryApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApi.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(string id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(string id);
        Task<IEnumerable<Order>> GetOrderAsync();
    }
}
