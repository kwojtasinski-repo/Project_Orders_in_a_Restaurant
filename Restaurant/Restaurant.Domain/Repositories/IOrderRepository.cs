using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Guid, Order>
    {
        Task<Order> AddAsync(Order entity);
        Order GetLatestOrderOnDate(DateTime currentDate);
        Task<Order> GetLatestOrderOnDateAsync(DateTime currentDate);
        Task DeleteAsync(Guid id);
        void DeleteOrders(IEnumerable<Guid> ids);
        Task<IEnumerable<Order>> GetAllAsync();
        Task DeleteOrdersAsync(IEnumerable<Guid> ids);
        Task<Order> GetAsync(Guid id);
    }
}
