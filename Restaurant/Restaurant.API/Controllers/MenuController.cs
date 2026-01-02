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
            var productsTask = _productService.GetAllAsync();
            var additionsTask = _additonService.GetAllAsync();
            await Task.WhenAll(productsTask, additionsTask);
            var menu = new MenuDto
            {
                Products = productsTask.Result,
                Additions = additionsTask.Result
            };
            return Ok(menu);
        }
    }
}
