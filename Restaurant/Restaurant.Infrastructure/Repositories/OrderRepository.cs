﻿using Dapper;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            var sql = "INSERT INTO orders (Id, OrderNumber, Created, Price, Email) VALUES (@Id, @OrderNumber, @Created, @Price, @Email)";
            _dbConnection.Execute(sql, entity);
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var sql = "DELETE FROM orders WHERE Id = @Id";
            _dbConnection.Execute(sql, new { Id = id });
        }

        public Order Get(Guid id)
        {
            var sql = @"SELECT * FROM orders o
                        LEFT JOIN order_product op ON op.OrderId = o.Id
                        LEFT JOIN products p ON p.Id = op.ProductId
                        WHERE o.Id = @Id";
            var result = _dbConnection.Query<Order, Product, Order>(sql,
                (order, product) => {
                    if (order?.Id != Guid.Empty)
                    {
                        order.Products.Add(product);
                    }
                    return order;
                },
                new { Id = id })
                .GroupBy(o => o.Id)
                .Select(group =>
                {
                    var combinedOwner = group.First();
                    combinedOwner.AddProducts(group.Select(owner => owner.Products.Single()));
                    return combinedOwner;
                });
            return result.SingleOrDefault();
        }

        public ICollection<Order> GetAll()
        {
            var sql = "SELECT * FROM orders";
            var result = _dbConnection.Query<Order>(sql);
            return result.ToList();
        }

        public void Update(Order entity)
        {
            var sql = "UPDATE orders SET OrderNumber = @OrderNumber, Created = @Created, Price = @Price, Email = @Email WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
