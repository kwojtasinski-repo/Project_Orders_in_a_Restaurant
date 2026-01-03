using Restaurant.ApplicationLogic.DTO;
using System.Collections.Generic;

namespace Restaurant.UI.DTO
{
    public class MenuDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public IEnumerable<AdditionDto> Additions { get; set; }
    }
}
