using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using DeliveryApi.Domain.Attributes;

namespace DeliveryApi.Domain.Entities
{
    [BsonCollection("orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; private set; }

        [BsonElement("customerName")]
        public string CustomerName { get; private set; }

        [BsonElement("status")]
        public bool Status { get; private set; }

        [BsonElement("items")]
        public List<OrderItem> Items { get; private set; }

        [BsonElement("createdAt")] 
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; private set; }

        public Order()
        {
            Id = Guid.NewGuid();
            CustomerName = string.Empty;
            Status = false;
            Items = new List<OrderItem>();
            CreatedAt = DateTime.UtcNow;
        }

        public Order(string customerName,bool status, List<OrderItem> items, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            CustomerName = customerName;
            Status = status;
            Items = items;
            CreatedAt = DateTime.UtcNow;
        }
    }
}