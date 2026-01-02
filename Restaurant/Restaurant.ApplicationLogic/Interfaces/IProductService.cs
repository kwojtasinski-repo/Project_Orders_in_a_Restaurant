using Restaurant.ApplicationLogic.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IProductService : IService
    {
        ProductDetailsDto Get(Guid id);
        IEnumerable<ProductDto> GetAll();
        Guid Add(ProductDto product);
        void Update(ProductDto product);
        void Delete(Guid id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
    }
}
