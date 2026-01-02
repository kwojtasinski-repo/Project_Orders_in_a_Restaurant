using Microsoft.AspNetCore.Mvc;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.API.DTO;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAdditonService _additonService;

        public MenuController(IProductService productService, IAdditonService additonService)
        {
            _productService = productService;
            _additonService = additonService;
        }

        [HttpGet]
        public async Task<ActionResult<MenuDto>> GetMenu()
        {
            var products = await Task.Run(() => _productService.GetAll());
            var additions = await Task.Run(() => _additonService.GetAll());
            var menu = new MenuDto
            {
                Products = products,
                Additions = additions
            };
            return Ok(menu);
        }
    }
}
