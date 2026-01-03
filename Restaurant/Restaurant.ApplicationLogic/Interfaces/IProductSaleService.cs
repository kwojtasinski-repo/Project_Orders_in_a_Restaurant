using Restaurant.Shared.DTO;
using System;
using System.Collections.Generic;

namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IProductSaleService : IService
    {
        Guid Add(ProductSaleDto productSaleDto);
        void Update(ProductSaleDto productSaleDto);
        IEnumerable<ProductSaleDto> GetAllByOrderId(Guid orderId);
    }
}
