using System.Collections.Concurrent;

namespace Catalogo.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfig config;
        readonly ConcurrentDictionary<string, CustomerLogger> Loggers = new
            ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfig config)
        {
            this.config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return Loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, config));
        }

        public void Dispose()
        {
            Loggers.Clear();
        }
    }
}
