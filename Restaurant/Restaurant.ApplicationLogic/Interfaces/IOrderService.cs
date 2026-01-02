using System.Collections.Generic;
using Restaurant.ApplicationLogic.DTO;
using System;

namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IOrderService : IService
    {
        OrderDetailsDto Get(Guid id);
        IEnumerable<OrderDto> GetAll();
        Guid Add(OrderDto order);
        Guid Add(OrderDetailsDto orderDetailsDto);
        void Update(OrderDto order);
        void Delete(Guid id);
        void DeleteOrders(IEnumerable<Guid> ids);
    }
}
