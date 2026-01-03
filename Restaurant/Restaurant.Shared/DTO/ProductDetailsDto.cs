using System.Collections.Generic;

namespace Restaurant.Shared.DTO
{
    public class ProductDetailsDto : ProductDto
    {
        public IEnumerable<OrderDto> Orders { get; set; }
    }
}
