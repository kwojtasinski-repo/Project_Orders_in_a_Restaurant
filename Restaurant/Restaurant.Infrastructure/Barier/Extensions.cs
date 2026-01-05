using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.Infrastructure.Barier
{
    internal static class Extensions
    {
        public static IServiceCollection AddStartupBarrier(this IServiceCollection services)
        {
            services.AddSingleton<IStartupBarrier, StartupBarrier>();
            return services;
        }
    }
}
