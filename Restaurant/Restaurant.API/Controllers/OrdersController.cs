using Microsoft.AspNetCore.Mvc;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Shared.DTO;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> Get(Guid id)
        {
            var order = await orderService.GetAsync(id);
            if (order is null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailsDto>> Add([FromBody] OrderDetailsDto orderDetailsDto)
        {
            var dto = await orderService.AddAsync(orderDetailsDto);
            return CreatedAtAction(nameof(Get), new { dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await orderService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("multi")]
        public async Task<IActionResult> DeleteOrders([FromBody] IEnumerable<Guid> ids)
        {
            await orderService.DeleteOrdersAsync(ids ?? []);
            return NoContent();
        }
    }
}
