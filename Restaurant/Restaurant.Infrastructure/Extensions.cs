using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Barier;
using Restaurant.Infrastructure.DAL;
using Restaurant.Infrastructure.DAL.Initialization;
using Restaurant.Infrastructure.Mappings;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Migrations.Orders;
using System;
using System.Data;

namespace Restaurant.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAdditonRepository, AdditonRepository>();
            services.AddScoped<IProductSaleRepository, ProductSaleRepository>();
            services.AddDbConnection(configuration);
            services.ApplyMappings();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddFluentMigratorCore()
                .ConfigureRunner(runner => runner.AddSQLite()
                                                 .WithGlobalCommandTimeout(TimeSpan.FromSeconds(5))
                                                 .WithGlobalConnectionString((_) => {
                                                     return configuration.GetConnectionString("RestaurantDb");
                                                 })
                                                 .ScanIn(typeof(InitCreateTableOrders_04_10_2022_18_05).Assembly).For.Migrations());
            services.AddStartupBarrier();
            services.AddHostedService<DbInitializer>();
            return services;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>((_) => {
                var connection = new SqliteConnection(configuration.GetConnectionString("RestaurantDb"));
                connection.Open();
                return connection;
            });
            return services;
        }
    }
}
