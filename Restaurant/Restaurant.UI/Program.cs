using Restaurant.UI.Services;
using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;

namespace Restaurant.UI
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = SetupApplication.Create();
            container.Register(Castle.MicroKernel.Registration.Component.For<HttpClient>()
                    .UsingFactoryMethod((_) => new HttpClient() { BaseAddress = new Uri("https://localhost:7040") })
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
                          .ReadFrom.AppSettings()
                          .CreateLogger();
                    }).LifestyleSingleton());
            var logger = container.Resolve<ILogger>();
            Application.EnableVisualStyles();
            // Add handler to handle the exception raised by main threads
            Application.ThreadException += new ThreadExceptionEventHandler((s, e) => ApplicationThreadException(s, e, logger));

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainPanel>());
        }

        // All exceptions thrown by the main thread are handled over this method
        static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e, ILogger logger)
        {
            var exception = e.Exception;
            logger.Error(exception, exception.Message);
            exception.MapToMessageBox();
        }
    }
}
