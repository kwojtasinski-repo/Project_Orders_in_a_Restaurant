using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.DTO;
using System;
using System.Collections.Generic;
using Restaurant.Domain.Repositories;
using System.Linq;
using Restaurant.ApplicationLogic.Mappings;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Implementation
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Guid Add(ProductDto product)
        {
            product.Id = Guid.NewGuid();
            var id = _productRepository.Add(product.AsEntity());
            return id;
        }

        public void Delete(Guid id)
        {
            _productRepository.Delete(id);
        }

        public ProductDetailsDto Get(Guid id)
        {
            var product = _productRepository.Get(id);
            return product.AsDetailsDto();
        }

        public IEnumerable<ProductDto> GetAll()
        {
            var products = _productRepository.GetAll();
            return products.Select(p => p.AsDto());
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => p.AsDto());
        }

        public void Update(ProductDto product)
        {
            _productRepository.Update(product.AsEntity());
        }
    }
}
