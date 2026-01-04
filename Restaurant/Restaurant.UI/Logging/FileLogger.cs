using Microsoft.Extensions.Logging;
using System.Text;

namespace Restaurant.UI.Logging
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _path;
        private readonly string _category;

        public FileLogger(string path, string category)
        {
            _path = path;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            var currentDateTime = DateTime.UtcNow;
            var line =
                $"{currentDateTime:yyyy-MM-dd HH:mm:ss.fff} " +
                $"[{logLevel}] ({_category}) {message}";

            if (exception != null)
            {
                line += Environment.NewLine + exception;
            }

            File.AppendAllText(GetFullPath(_path, currentDateTime), line + Environment.NewLine, Encoding.UTF8);
        }

        private static string GetFullPath(string path, DateTime date)
        {
            var lastDotChar = path.Split('.').LastOrDefault();
            if (lastDotChar is null)
            {
                path += $"log_{date:yyyy_MM_dd}.txt";
            }
            else
            {
                path = path.Replace($".{lastDotChar}", $"_log_{date:yyyy_MM_dd}.{lastDotChar}");
            }

            return path;
        }
    }
}
