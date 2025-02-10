using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApi.Application.DTOs
{
    public class OrderDto
    {
        public string? Id { get; set; }
        public string CustomerName { get; set; }
        public bool Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
