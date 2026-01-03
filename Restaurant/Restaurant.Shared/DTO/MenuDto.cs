using System.Collections.Generic;

namespace Restaurant.Shared.DTO
{
    public class MenuDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public IEnumerable<AdditionDto> Additions { get; set; }
    }
}
