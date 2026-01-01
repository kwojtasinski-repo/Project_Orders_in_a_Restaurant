using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic.Exceptions;
using Restaurant.ApplicationLogic.Implementation;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mail;

namespace Restaurant.ApplicationLogic
{
    public static class Extensions
    {
        public static IWindsorContainer AddApplicationLogic(this IWindsorContainer container)
        {
            container.Register(Component.For<IProductService>().ImplementedBy<ProductService>().LifestyleTransient());
            container.Register(Component.For<IOrderService>().ImplementedBy<OrderService>().LifestyleTransient());
            container.Register(Component.For<IAdditonService>().ImplementedBy<AdditonService>().LifestyleTransient());
            container.Register(Component.For<IMailSender>().ImplementedBy<MailSender>().LifestyleTransient());
            container.Register(Component.For<IOptions>().ImplementedBy<Options>().LifestyleSingleton());
            container.Register(Component.For<IProductSaleService>().ImplementedBy<ProductSaleService>().LifestyleTransient());
            container.Register(Component.For<IMapToApplicationException>().ImplementedBy<MapToApplicationException>().LifestyleSingleton());
            return container;
        }

        public static IServiceCollection AddApplicationLogic(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAdditonService, AdditonService>();
            services.AddTransient<IMailSender, MailSender>();
            services.AddScoped<IOptions, Options>();
            services.AddTransient<IProductSaleService, ProductSaleService>();
            services.AddSingleton<IMapToApplicationException, MapToApplicationException>();
            return services;
        }
    }
}
