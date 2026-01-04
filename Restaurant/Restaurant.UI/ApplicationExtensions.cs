using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurant.UI.Logging;
using Restaurant.UI.Services;

namespace Restaurant.UI
{
    internal static class ApplicationExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton((_) => new HttpClient() { BaseAddress = new Uri(configuration.GetSection("BackendUrl")?.Value ?? throw new InvalidOperationException("Put backend url to appsettings.json to root `\"BackendUrl\": \"URL\"`")) });
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<MainPanel>();
            services.AddSingleton<Menu>();
            services.AddSingleton<Settings>();
            services.AddSingleton<History>();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddProvider(
                    new FileLoggerProvider(
                        configuration["Logging:File:Path"]!
                    )
                );

            });
            return services;
        }
    }
}
