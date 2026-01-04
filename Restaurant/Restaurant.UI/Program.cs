using Microsoft.Extensions.Configuration;
using Castle.Windsor;
using Restaurant.UI.ErrorHandling;
using Serilog;

namespace Restaurant.UI
{
    static class Program
    {
        public static IConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var container = new WindsorContainer();
            container.RegisterServices(Configuration);
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
