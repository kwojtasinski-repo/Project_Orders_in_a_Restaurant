using Microsoft.AspNetCore.Mvc;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.DTO;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await Task.Run(() => _orderService.GetAll());
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> Get(Guid id)
        {
            var order = await Task.Run(() => _orderService.Get(id));
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] OrderDetailsDto orderDetailsDto)
        {
            var id = await Task.Run(() => _orderService.Add(orderDetailsDto));
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Run(() => _orderService.Delete(id));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrders([FromBody] IEnumerable<Guid> ids)
        {
            await Task.Run(() => _orderService.DeleteOrders(ids));
            return NoContent();
        }
    }
}
