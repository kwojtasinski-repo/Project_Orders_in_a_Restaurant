using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.DAL;
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
                                                 .WithGlobalConnectionString(configuration.GetConnectionString("RestaurantDb"))
                                                 .ScanIn(typeof(InitCreateTableOrders_04_10_2022_18_05).Assembly).For.Migrations());
            return services;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RestaurantDb");
            services.AddScoped<IDbConnection>((_) => {
                var connection = new SqliteConnection(connectionString);
                connection.Open();
                return connection;
            });
            return services;
        }

        public static void UseInfrastructure(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                migrationRunner.MigrateUp();
            }
        }
    }
}
