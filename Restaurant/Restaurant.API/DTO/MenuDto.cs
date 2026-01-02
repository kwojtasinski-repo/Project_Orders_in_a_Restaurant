using System.Collections.Generic;
using Restaurant.ApplicationLogic.DTO;

namespace Restaurant.API.DTO
{
    public class MenuDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public IEnumerable<AdditionDto> Additions { get; set; }
    }
}
