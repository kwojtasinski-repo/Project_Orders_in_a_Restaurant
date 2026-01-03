using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic;
using Restaurant.Infrastructure;
using System;
using System.Collections.Generic;

namespace Restaurant.IntegrationTests.Common
{
    internal class TestApplicationFactory
    {
        public static string DB_FILE_NAME => "restaurant-integration-test.db";

        public IServiceProvider StartApplication()
        {
            var services = new ServiceCollection();
            var inMemorySettings = new Dictionary<string, string>
            {
                ["ConnectionStrings:RestaurantDb"] = $"Data Source={DB_FILE_NAME};New=True;BinaryGuid=False",
                ["EmailOptions:Login"] = "login",
                ["EmailOptions:Password"] = "",
                ["EmailOptions:SmtpClient"] = "",
                ["EmailOptions:SmtpPort"] = "578",
                ["EmailOptions:Email"] = "email@email.com"
            };


            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(inMemorySettings);
            var configuration = configurationBuilder.Build();

            services.AddApplicationLogic(configuration);
            services.AddInfrastructure(configuration);

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.UseInfrastructure();
            DataSeed.AddData(serviceProvider);
            return serviceProvider;
        }
    }
}
