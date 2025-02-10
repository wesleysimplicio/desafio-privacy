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
        Task<Order> GetByIdAsync(Object id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Object id);
        Task<IEnumerable<Order>> GetOrderAsync();
    }
}
