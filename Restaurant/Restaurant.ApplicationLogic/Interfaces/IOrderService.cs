using System.Collections.Generic;
using Restaurant.ApplicationLogic.DTO;
using System;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IOrderService : IService
    {
        OrderDetailsDto Get(Guid id);
        Task<OrderDetailsDto> GetAsync(Guid id);
        IEnumerable<OrderDto> GetAll();
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Guid Add(OrderDto order);
        Guid Add(OrderDetailsDto orderDetailsDto);
        Task<OrderDetailsDto> AddAsync(OrderDetailsDto orderDetailsDto);
        void Update(OrderDto order);
        void Delete(Guid id);
        Task DeleteAsync(Guid id);
        void DeleteOrders(IEnumerable<Guid> ids);
        Task DeleteOrdersAsync(IEnumerable<Guid> ids);
    }
}
