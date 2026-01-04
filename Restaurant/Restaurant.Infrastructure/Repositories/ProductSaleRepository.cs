using Dapper;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Restaurant.Infrastructure.Repositories
{
    internal class ProductSaleRepository : IProductSaleRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductSaleRepository(IUnitOfWork unitOfWork)
        {
            _dbConnection = unitOfWork.Connection;
        }

        public Guid Add(ProductSale entity)
        {
            var sql = "INSERT INTO product_sales (Id, ProductId, OrderId, AdditionId, EndPrice, Email, ProductSaleState) VALUES (@Id, @ProductId, @OrderId, @AdditionId, @EndPrice, @Email, @ProductSaleState)";
            _dbConnection.Execute(sql, entity);
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var sql = "DELETE FROM product_sales WHERE Id = @Id";
            _dbConnection.Execute(sql, new { Id = id });
        }

        public ProductSale Get(Guid id)
        {
            var sql = @"SELECT ps.*, p.*, a.*, o.* FROM product_sales ps
                        LEFT JOIN products p ON ps.ProductId = p.Id
                        LEFT JOIN additions a on ps.AdditionId = a.Id
                        LEFT JOIN orders o ON o.Id = ps.OrderId
                        WHERE p.Id = @Id";
            var result = _dbConnection.Query<ProductSalePOCO, ProductPOCO, AdditionPOCO, OrderPOCO, ProductSalePOCO>(sql,
                (productSale, product, addition, order) => {
                    if (order != null && order.Id != Guid.Empty)
                    {
                        if (product != null && product.Id != Guid.Empty)
                        {
                            if (addition != null && addition.Id != Guid.Empty)
                            {
                                productSale.Addition = addition;
                            }
                            productSale.Product = product;
                        }

                        productSale.Order = order;
                    }
                    return productSale;
                },
                new { Id = id });

            var productToReturn = result.SingleOrDefault();
            return productToReturn?.AsDetailsEntity();
        }

        public ICollection<ProductSale> GetAll()
        {
            var sql = "SELECT * FROM product_sales";
            var result = _dbConnection.Query<ProductSalePOCO>(sql);
            return result.Select(p => p.AsEntity()).ToList();
        }

        public IEnumerable<ProductSale> GetAllByOrderId(Guid orderId)
        {
            var sql = @"SELECT ps.*, p.*, a.* FROM product_sales ps
                        JOIN products p ON ps.ProductId = p.Id
                        LEFT JOIN additions a on ps.AdditionId = a.Id
                        WHERE OrderId = @OrderId";
            var result = _dbConnection.Query<ProductSalePOCO, ProductPOCO, AdditionPOCO, ProductSalePOCO>(sql,
                (productSale, product, addition) => {
                    if (addition != null && addition.Id != Guid.Empty)
                    {
                        productSale.Addition = addition;
                    }
                    productSale.Product = product;
                    return productSale;
                },
                new { OrderId = orderId });
            return result.Select(p => p.AsEntity()).ToList();
        }

        public void Update(ProductSale entity)
        {
            var sql = "UPDATE product_sales SET Id = @Id, ProductId = @ProductId, OrderId = @OrderId, AdditionId = @AdditionId, EndPrice = @EndPrice, Email = @Email, ProductSaleState = @ProductSaleState";
            _dbConnection.Execute(sql, entity);
        }
    }
}
