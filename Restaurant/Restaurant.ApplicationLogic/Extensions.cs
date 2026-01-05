using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.ApplicationLogic.Exceptions;
using Restaurant.ApplicationLogic.Implementation;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mail;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Restaurant.UnitTests")]
namespace Restaurant.ApplicationLogic
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAdditonService, AdditonService>();
            services.AddTransient<IMailSender, MailSender>();
            services.AddScoped<IEmailOptions, EmailOptions>();
            services.AddTransient<IProductSaleService, ProductSaleService>();
            services.AddSingleton<IMapToApplicationException, MapToApplicationException>();
            var emailOptions = new EmailOptions();
            configuration.GetSection(nameof(EmailOptions)).Bind(emailOptions);
            services.AddSingleton(emailOptions);
            services.AddSingleton<IEmailOptions>(sp => sp.GetRequiredService<EmailOptions>());
            return services;
        }
    }
}
