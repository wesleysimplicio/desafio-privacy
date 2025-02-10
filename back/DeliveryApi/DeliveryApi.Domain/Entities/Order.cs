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
        public ObjectId Id { get; private set; }

        [BsonElement("customerName")]
        public string CustomerName { get; private set; }

        [BsonElement("status")]
        public bool Status { get; private set; }

        [BsonElement("items")]
        public List<OrderItem> Items { get; private set; } = new();

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedAt { get; private set; }

        [BsonIgnore]
        public int __v { get; set; }

        public Order()
        {
            Id = ObjectId.GenerateNewId();
            CustomerName = string.Empty;
            Status = false;
            Items = new List<OrderItem>();
            CreatedAt = DateTime.UtcNow;
        }

        public Order(string customerName, bool status, List<OrderItem> items, ObjectId? id = null,DateTime? createdAt = null)
        {
            Id = id ?? ObjectId.GenerateNewId();
            CustomerName = customerName;
            Status = status;
            Items = items;
            CreatedAt = createdAt ?? DateTime.UtcNow;
        }
    }
}