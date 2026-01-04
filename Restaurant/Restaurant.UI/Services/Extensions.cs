using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Restaurant.UI.Services
{
    internal static class Extensions
    {
        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            return services;
        }
    }
}
