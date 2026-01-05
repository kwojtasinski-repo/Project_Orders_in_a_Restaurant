using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using System;
using System.Data;

namespace Restaurant.IntegrationTests.Common
{
    internal class DataSeed
    {
        public static void AddData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            dbConnection.Execute("INSERT INTO products (Id, ProductName, Price, ProductKind) VALUES(@Id, @ProductName, @Price, @ProductKind)",
                new
                {
                    Id = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                    ProductName = "Product A #1",
                    Price = 500.00M,
                    ProductKind = ProductKind.Pizza
                });

            dbConnection.Execute("INSERT INTO products (Id, ProductName, Price, ProductKind) VALUES(@Id, @ProductName, @Price, @ProductKind)",
                new
                {
                    Id = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                    ProductName = "Product B #1",
                    Price = 500.00M,
                    ProductKind = ProductKind.Drink
                });

            dbConnection.Execute("INSERT INTO orders (Id, Created, Email, OrderNumber, Price) VALUES(@Id, @Created, @Email, @OrderNumber, @Price)",
                new
                {
                    Id = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                    Created = DateTime.UtcNow,
                    Email = "email@email.com",
                    OrderNumber = "ORD/1",
                    Price = 1000.00M
                });

            dbConnection.Execute("INSERT INTO orders (Id, Created, Email, OrderNumber, Price) VALUES(@Id, @Created, @Email, @OrderNumber, @Price)",
                new
                {
                    Id = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                    Created = DateTime.UtcNow,
                    Email = "email@email.com",
                    OrderNumber = "ORD/2",
                    Price = 2000.00M
                });

            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       ProductId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       OrderId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });
            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("90d5f825-0e1e-436e-8013-e8f631be17b3"),
                                       ProductId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       OrderId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });

            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       ProductId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       OrderId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });

            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("99c2e948-e668-4dc0-a338-6e065ad04e5a"),
                                       ProductId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd"),
                                       OrderId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });

            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("75fcb257-0f98-47ef-ae8e-af7d371e7385"),
                                       ProductId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       OrderId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });

            dbConnection.Execute(@"INSERT INTO product_sales (Id, ProductId, OrderId, EndPrice, ProductSaleState, Email) 
                                   VALUES(@Id, @ProductId, @OrderId, @EndPrice, @ProductSaleState, @Email)",
                                   new
                                   {
                                       Id = new Guid("e070ca3b-71ae-47b3-b8eb-0c5afa5b6bb5"),
                                       ProductId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       OrderId = new Guid("b9302f85-9fcc-446f-9dd3-f8510fc864b9"),
                                       EndPrice = 500.00M,
                                       ProductSaleState = ProductSaleState.New,
                                       Email = "email@email.com"
                                   });
        }
    }
}
