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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _dbConnection;

        public OrderRepository(IUnitOfWork unitOfWork)
        {
            _dbConnection = unitOfWork.Connection;
        }

        public Guid Add(Order entity)
        {
            var sql = "INSERT INTO orders (Id, OrderNumber, Created, Price, Email, Note) VALUES (@Id, @OrderNumber, @Created, @Price, @Email, @Note)";
            _dbConnection.Execute(sql, entity);
            return entity.Id;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            var sql = "INSERT INTO orders (Id, OrderNumber, Created, Price, Email, Note) VALUES (@Id, @OrderNumber, @Created, @Price, @Email, @Note)";
            await _dbConnection.ExecuteAsync(sql, entity);
            return entity;
        }

        public void Delete(Guid id)
        {
            var sqlProductSales = "DELETE FROM product_sales WHERE OrderId = @OrderId";
            _dbConnection.Execute(sqlProductSales, new { OrderId = id });
            var sql = "DELETE FROM orders WHERE Id = @Id";
            _dbConnection.Execute(sql, new { Id = id });
        }

        public async Task DeleteAsync(Guid id)
        {
            var sqlProductSales = "DELETE FROM product_sales WHERE OrderId = @OrderId";
            await _dbConnection.ExecuteAsync(sqlProductSales, new { OrderId = id });
            var sql = "DELETE FROM orders WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public void DeleteOrders(IEnumerable<Guid> ids)
        {
            var (inClause, parameters) = SetSqlParametersInClause("id", ids);
            var sqlProductSales = $"DELETE FROM product_sales WHERE OrderId IN ({inClause})";
            _dbConnection.Execute(sqlProductSales, parameters);
            var sqlOrders = $"DELETE FROM product_sales WHERE OrderId IN ({inClause})";
            _dbConnection.Execute(sqlOrders, parameters);
        }

        public async Task DeleteOrdersAsync(IEnumerable<Guid> ids)
        {
            var (inClause, parameters) = SetSqlParametersInClause("id", ids);
            var sqlProductSales = $"DELETE FROM product_sales WHERE OrderId IN ({inClause})";
            await _dbConnection.ExecuteAsync(sqlProductSales, parameters);
            var sqlOrders = $"DELETE FROM orders WHERE Id IN ({inClause})";
            await _dbConnection.ExecuteAsync(sqlOrders, parameters);
        }

        public Order Get(Guid id)
        {
            var sql = @"SELECT
                            o.Id, o.OrderNumber, o.Created, o.Price, o.Email, o.Note,
                            ps.Id, ps.OrderId, ps.ProductId, ps.AdditionId, ps.EndPrice, ps.ProductSaleState, ps.Email,
                            a.Id, a.AdditionName, a.Price, a.ProductKind,
                            p.Id, p.ProductName, p.Price, p.ProductKind
                        FROM orders o
                        LEFT JOIN product_sales ps ON ps.OrderId = o.Id
                        LEFT JOIN additions a on ps.AdditionId = a.Id
                        LEFT JOIN products p ON p.Id = ps.ProductId
                        WHERE o.Id = @Id";
            var result = _dbConnection.Query<OrderPOCO, ProductSalePOCO, AdditionPOCO, ProductPOCO, OrderPOCO>(sql,
                (order, productSale, addition, product) => {
                    if (productSale != null && productSale.Id != Guid.Empty)
                    {
                        if(addition != null && addition.Id != Guid.Empty)
                        {
                            productSale.Addition = addition;
                        }
                        productSale.Product = product;
                        order.Products.Add(productSale);
                    }
                    return order;
                },
                new { Id = id })
                .GroupBy(o => o.Id)
                .Select(group =>
                {
                    var combinedOwner = group.First();
                    var products = group.Select(owner => owner.Products.SingleOrDefault()).ToList();
                    
                    if (products.Any(p => p is null))
                    {
                        return combinedOwner;
                    }

                    foreach(var product in products)
                    {
                        combinedOwner.Products.Add(product);
                    }

                    combinedOwner.Products = combinedOwner.Products.Distinct().ToList();
                    
                    return combinedOwner;
                });

            var orderToReturn = result.SingleOrDefault();
            return orderToReturn?.AsDetailsEntity();
        }

        public ICollection<Order> GetAll()
        {
            var sql = "SELECT Id, OrderNumber, Created, Price, Email, Note FROM orders";
            var result = _dbConnection.Query<OrderPOCO>(sql);
            return result.Select(o => o.AsEntity()).ToList();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var sql = "SELECT Id, OrderNumber, Created, Price, Email, Note FROM orders";
            var result = await _dbConnection.QueryAsync<OrderPOCO>(sql);
            return result.Select(o => o.AsEntity()).ToList();
        }

        public async Task<Order> GetAsync(Guid id)
        {
            var sql = @"SELECT
                            o.Id, o.OrderNumber, o.Created, o.Price, o.Email, o.Note,
                            ps.Id, ps.OrderId, ps.ProductId, ps.AdditionId, ps.EndPrice, ps.ProductSaleState, ps.Email,
                            a.Id, a.AdditionName, a.Price, a.ProductKind,
                            p.Id, p.ProductName, p.Price, p.ProductKind
                        FROM orders o
                        LEFT JOIN product_sales ps ON ps.OrderId = o.Id
                        LEFT JOIN additions a on ps.AdditionId = a.Id
                        LEFT JOIN products p ON p.Id = ps.ProductId
                        WHERE o.Id = @Id";
            var result = (await _dbConnection.QueryAsync<OrderPOCO, ProductSalePOCO, AdditionPOCO, ProductPOCO, OrderPOCO>(sql,
                (order, productSale, addition, product) => {
                    if (productSale != null && productSale.Id != Guid.Empty)
                    {
                        if(addition != null && addition.Id != Guid.Empty)
                        {
                            productSale.Addition = addition;
                        }
                        productSale.Product = product;
                        order.Products.Add(productSale);
                    }
                    return order;
                },
                new { Id = id }))
                .GroupBy(o => o.Id)
                .Select(group =>
                {
                    var combinedOwner = group.First();
                    var products = group.Select(owner => owner.Products.SingleOrDefault()).ToList();
                    
                    if (products.Any(p => p is null))
                    {
                        return combinedOwner;
                    }

                    foreach(var product in products)
                    {
                        combinedOwner.Products.Add(product);
                    }

                    combinedOwner.Products = combinedOwner.Products.Distinct().ToList();
                    
                    return combinedOwner;
                });

            var orderToReturn = result.SingleOrDefault();
            return orderToReturn?.AsDetailsEntity();
        }

        public Order GetLatestOrderOnDate(DateTime currentDate)
        {
            var sql = @"SELECT o.Id, o.OrderNumber, o.Created, o.Price, o.Email, o.Note FROM orders o
                        WHERE Datetime(o.Created) > @CurrentDate
                        ORDER BY o.Created DESC
                        LIMIT 1";
            var result = _dbConnection.Query<OrderPOCO>(sql, new
            {
                CurrentDate = currentDate.Date
            }).SingleOrDefault();
            return result?.AsEntity();
        }

        public async Task<Order> GetLatestOrderOnDateAsync(DateTime currentDate)
        {
            var sql = @"SELECT o.Id, o.OrderNumber, o.Created, o.Price, o.Email, o.Note FROM orders o
                        WHERE Datetime(o.Created) > @CurrentDate
                        ORDER BY o.Created DESC
                        LIMIT 1";
            var result = (await _dbConnection.QueryAsync<OrderPOCO>(sql, new
            {
                CurrentDate = currentDate.Date
            })).SingleOrDefault();
            return result?.AsEntity();
        }

        public void Update(Order entity)
        {
            var sql = "UPDATE orders SET OrderNumber = @OrderNumber, Created = @Created, Price = @Price, Email = @Email WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }

        private (string InClause, DynamicParameters Parameters) SetSqlParametersInClause<T>(string parameterPrefix, IEnumerable<T> values)
        {
            var list = values?.ToList() ?? throw new ArgumentNullException(nameof(values));
            if (!list.Any())
                throw new ArgumentException("Values collection cannot be empty", nameof(values));

            var parameters = new DynamicParameters();
            var names = new List<string>();

            for (var i = 0; i < list.Count; i++)
            {
                var name = $"{parameterPrefix}{i}";
                names.Add($"@{name}");
                parameters.Add(name, list[i]);
            }

            return (string.Join(", ", names), parameters);
        }
    }
}
