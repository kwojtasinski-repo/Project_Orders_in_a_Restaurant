using Microsoft.Extensions.Logging;

namespace Restaurant.UI.Logging
{
    [ProviderAlias("File")]
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
            => new FileLogger(_path, categoryName);

        public void Dispose() { }
    }
}
