using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Guid, Order>
    {
        Order GetLatestOrderOnDateAsync(DateTime currentDate);
        void DeleteOrders(IEnumerable<Guid> ids);
    }
}
