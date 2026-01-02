using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IProductRepository : IRepository<Guid, Product>
    {
        Task<ICollection<Product>> GetAllAsync();
    }
}
