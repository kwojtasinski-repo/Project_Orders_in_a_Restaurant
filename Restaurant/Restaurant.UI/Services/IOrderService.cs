using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.ApplicationLogic.DTO;
using Restaurant.UI.DTO;

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
