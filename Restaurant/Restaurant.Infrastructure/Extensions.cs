using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.DAL;
using Restaurant.Infrastructure.Mappings;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Infrastructure.Requests;
using Restaurant.Migrations.Orders;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;

namespace Restaurant.Infrastructure
{
    public static class Extensions
    {
        public static IWindsorContainer AddInfrastructure(this IWindsorContainer container, NameValueCollection appSettings)
        {
            container.Register(Component.For<IOrderRepository>().ImplementedBy<OrderRepository>().LifestyleScoped());
            container.Register(Component.For<IProductRepository>().ImplementedBy<ProductRepository>().LifestyleScoped());
            container.Register(Component.For<IAdditonRepository>().ImplementedBy<AdditonRepository>().LifestyleScoped());
            container.Register(Component.For<IProductSaleRepository>().ImplementedBy<ProductSaleRepository>().LifestyleScoped());
            container.AddDbConnection(appSettings);
            container.Register(Component.For<IRequestHandler>()
                        .UsingFactoryMethod(factory =>
                        {
                            return new RequestHandler(container);
                        }).LifestyleSingleton());
            container.ApplyMappings();
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestyleScoped());
            container.AddFluentMigrator(appSettings["RestaurantDb"]);

            return container;
        }

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
                                                 .WithMigrationsIn(typeof(InitCreateTableOrders_04_10_2022_18_05).Assembly));
            return services;
        }

        public static IWindsorContainer AddDbConnection(this IWindsorContainer container, NameValueCollection appSettings)
        {
            var connectionString = appSettings["RestaurantDb"];
            container.Register(Component.For<IDbConnection>()
                        .UsingFactoryMethod(kernel => {
                                var connection = new SQLiteConnection(connectionString);
                                connection.Open();
                                return connection;
                            })
                        .LifestyleScoped());
            return container;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RestaurantDb");
            services.AddScoped<IDbConnection>((_) => {
                var connection = new SQLiteConnection(connectionString);
                connection.Open();
                return connection;
            });
            return services;
        }

        private static IWindsorContainer AddFluentMigrator(this IWindsorContainer container, string connectionString)
        {
#pragma warning disable CS0612 // old style initialized New is initialized by ServiceProvider
            var announcer = new ConsoleAnnouncer()
            {
                ShowSql = true,
            };
           var options = new ProcessorOptions()
            {
                ConnectionString = connectionString,
                Timeout = TimeSpan.FromSeconds(5)
            };
            var processorFactory = new SQLiteProcessorFactory();
            var context = new RunnerContext(announcer)
            {
                AllowBreakingChange = true,
                Timeout = 5,
            };
            container.Register(Component.For<IMigrationProcessor>()
                        .UsingFactoryMethod(factory =>
                        {
                            return processorFactory.Create(connectionString, announcer, options);
                        }).LifestyleTransient());
            container.Register(Component.For<MigrationRunner>()
                        .UsingFactoryMethod(factory =>
                        {
                            var runner = new MigrationRunner(
                                typeof(InitCreateTableOrders_04_10_2022_18_05).Assembly,
                                context,
                                factory.Resolve<IMigrationProcessor>());
                            return runner;
                        }).LifestyleTransient());
#pragma warning restore CS0612 // old style initialized New is initialized by ServiceProvider

            return container;
        }

        public static void UseInfrastructure(this IWindsorContainer container)
        {
            using (var dispose = container.BeginScope())
            {
                var migrationRunner = container.Resolve<MigrationRunner>();
                migrationRunner.MigrateUp();
            }
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
