using Castle.Windsor;
using Microsoft.Extensions.Configuration;
using Restaurant.UI.Services;
using Serilog;

namespace Restaurant.UI
{
    internal static class ApplicationExtensions
    {
        public static WindsorContainer RegisterServices(this WindsorContainer container, IConfiguration configuration)
        {
            container.Register(Castle.MicroKernel.Registration.Component.For<HttpClient>()
                    .UsingFactoryMethod((_) => new HttpClient() { BaseAddress = new Uri(configuration.GetSection("BackendUrl")?.Value ?? throw new InvalidOperationException("Put backend url to appsettings.json to root `\"BackendUrl\": \"URL\"`")) })
                    .LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<IMenuService, MenuService>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<IOrderService, OrderService>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<ISettingsService, SettingsService>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<MainPanel>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<Menu>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<Settings>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<History>().LifestyleSingleton());
            container.Register(Castle.MicroKernel.Registration.Component.For<ILogger>().
                    UsingFactoryMethod(kernel =>
                    {
                        return new LoggerConfiguration()
                          .ReadFrom.Configuration(configuration)
                          .CreateLogger();
                    }).LifestyleSingleton());
            var logger = container.Resolve<ILogger>();
            return container;
        }
    }
}
