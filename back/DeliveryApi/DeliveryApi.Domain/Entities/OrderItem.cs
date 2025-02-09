using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApi.Domain.Entities
{
    public class OrderItem
    {
        [BsonElement("productName")]
        public string ProductName { get; private set; }

        [BsonElement("quantity")]
        public int Quantity { get; private set; }

        [BsonElement("price")]
        public decimal Price { get; private set; }

        public OrderItem(string productName, int quantity, decimal price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
