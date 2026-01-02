using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IAdditonRepository : IRepository<Guid, Addition>
    {
        Task<ICollection<Addition>> GetAllAsync();
    }
}
