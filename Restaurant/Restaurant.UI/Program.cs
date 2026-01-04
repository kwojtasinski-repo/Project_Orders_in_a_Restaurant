using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurant.UI.ErrorHandling;

namespace Restaurant.UI
{
    static class Program
    {
        public static IConfiguration Configuration { get; private set; } = null!;
        public static string EnvironmentName { get; private set; } = null!;
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var container = new ServiceCollection();
            container.RegisterServices(Configuration);
            ServiceProvider = container.BuildServiceProvider();

            var logger = ServiceProvider.GetRequiredService<ILogger<MainPanel>>();
            Application.EnableVisualStyles();
            // Add handler to handle the exception raised by main threads
            Application.ThreadException += new ThreadExceptionEventHandler((s, e) => ApplicationThreadException(s, e, logger));

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ServiceProvider.GetRequiredService<MainPanel>());
        }

        // All exceptions thrown by the main thread are handled over this method
        static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e, ILogger logger)
        {
            var exception = e.Exception;
            logger.LogError(exception, exception.Message);
            exception.MapToMessageBox();
        }
    }
}
