using Restaurant.Shared.DTO;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mappings;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.ApplicationLogic.Implementation
{
    internal class ProductSaleService : IProductSaleService
    {
        private readonly IProductSaleRepository _productSaleRepository;

        public ProductSaleService(IProductSaleRepository productSaleRepository)
        {
            _productSaleRepository = productSaleRepository;
        }

        public Guid Add(ProductSaleDto productSaleDto)
        {
            productSaleDto.Id = Guid.NewGuid();
            var id = _productSaleRepository.Add(productSaleDto.AsEntity());
            return id;
        }

        public void Update(ProductSaleDto productSaleDto)
        {
            _productSaleRepository.Update(productSaleDto.AsEntity());
        }

        public IEnumerable<ProductSaleDto> GetAllByOrderId(Guid orderId)
        {
            var products = _productSaleRepository.GetAllByOrderId(orderId);
            return products.Select(p => p.AsDto());
        }
    }
}
