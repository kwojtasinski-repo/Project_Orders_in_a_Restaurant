using Restaurant.Shared.DTO;

namespace Restaurant.UI.Services
{
    public interface IOrderService
    {
        Task<ApiResult<OrderDetailsDto>> AddOrderAsync(OrderDetailsDto order);
        Task<ApiResult<OrderDetailsDto>> GetOrderAsync(Guid id);
        Task<ApiResult<List<OrderDto>>> GetAllOrdersAsync();
        Task<ApiResult> DeleteOrders(List<Guid> orderIds);
    }
}
