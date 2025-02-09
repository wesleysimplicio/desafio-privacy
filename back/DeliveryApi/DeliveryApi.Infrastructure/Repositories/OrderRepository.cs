using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApi.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _orders.DeleteOneAsync(o => o.Id == id);
        }
    }
}
