using DeliveryApi.Domain.Entities;
using DeliveryApi.Domain.Repositories;
using MongoDB.Bson;
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
            _orders = database.GetCollection<Order>("orders");
        }

        public async Task<Order> GetByIdAsync(Object id)
        {
            return await _orders.Find(o => o.Id.ToString() == id.ToString()).Project<Order>(Builders<Order>.Projection.Exclude("__v")).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrderAsync()
        {
            return await _orders.Find(o => true).Project<Order>(Builders<Order>.Projection.Exclude("__v")).ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        public async Task DeleteAsync(Object id)
        {
            await _orders.DeleteOneAsync(o => o.Id.ToString() == id.ToString());
        }
    }
}
