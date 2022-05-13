using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class OrderDto
    {
        public OrderDto()
        {
            Items = new List<OrderItemDto>();
        }

        public List<OrderItemDto> Items { get; set; }
    }
}
