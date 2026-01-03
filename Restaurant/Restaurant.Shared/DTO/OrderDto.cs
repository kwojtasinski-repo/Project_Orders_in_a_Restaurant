using System;

namespace Restaurant.Shared.DTO
{
    public class OrderDto : BaseDto
    {
        public string OrderNumber { get; set; }
        public DateTime Created { get; set; }
        public decimal Price { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
