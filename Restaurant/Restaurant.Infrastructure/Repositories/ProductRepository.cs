using Dapper;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductRepository(IUnitOfWork unitOfWork)
        {
            _dbConnection = unitOfWork.Connection;
        }

        public Guid Add(Product entity)
        {
            var sql = "INSERT INTO products (Id, ProductName, Price, ProductKind) VALUES (@Id, @ProductName, @Price, @ProductKind)";
            _dbConnection.Execute(sql, entity);
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var sql = "DELETE FROM products WHERE Id = @Id";
            _dbConnection.Execute(sql, new { Id = id });
        }

        public Product Get(Guid id)
        {
            var sql = @"SELECT p.*, ps.*, a.*, o.* FROM products p
                        LEFT JOIN product_sales ps ON ps.ProductId = p.Id
                        LEFT JOIN additions a on ps.AdditionId = a.Id
                        LEFT JOIN orders o ON o.Id = ps.OrderId
                        WHERE p.Id = @Id";
            var result = _dbConnection.Query<ProductPOCO, ProductSalePOCO, AdditionPOCO, OrderPOCO, ProductPOCO>(sql,
                (product, productSale, addition, order) => { 
                    if (order != null && order.Id != Guid.Empty) {
                        if (productSale != null && productSale.Id != Guid.Empty)
                        {
                            if (addition != null && addition.Id != Guid.Empty)
                            {
                                productSale.Addition = addition;
                            }
                            productSale.Product = product;
                            order.Products.Add(productSale);
                        }

                        product.Orders.Add(order);
                    }
                    return product; },
                new { Id = id })
                .GroupBy(o => o.Id)
                .Select(group =>
                {
                    var combinedOwner = group.First();
                    var orders = group.Select(owner => owner.Orders.SingleOrDefault()).ToList();

                    if (orders.Any(o => o is null))
                    {
                        return combinedOwner;
                    }

                    foreach (var order in orders)
                    {
                        combinedOwner.Orders.Add(order);
                    }
                    return combinedOwner;
                });
            
            var productToReturn = result.SingleOrDefault();
            return productToReturn?.AsDetailsEntity();
        }

        public ICollection<Product> GetAll()
        {
            var sql = @"SELECT 
                        Id, ProductName, Price, ProductKind 
                        FROM products";
            var result = _dbConnection.Query<ProductPOCO>(sql);
            return result.Select(p => p.AsEntity()).ToList();
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            var sql = @"SELECT 
                        Id, ProductName, Price, ProductKind 
                        FROM products";
            var result = await _dbConnection.QueryAsync<ProductPOCO>(sql);
            return result.Select(p => p.AsEntity()).ToList();
        }

        public void Update(Product entity)
        {
            var sql = "UPDATE products SET ProductName = @ProductName, Price = @Price, ProductKind = @ProductKind WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
