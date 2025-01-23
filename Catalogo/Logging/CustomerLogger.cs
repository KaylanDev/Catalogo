
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Catalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string LoggerName;
        readonly CustomLoggerProviderConfig LoggerConfig;

        public CustomerLogger(string loggerName, CustomLoggerProviderConfig loggerConfig)
        {
            LoggerName = loggerName;
            LoggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LoggerConfig.LogLever;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string Message = $"{logLevel.ToString()} : {eventId.Id} - {formatter(state,exception)}";
            EscreverTextoNoArquivo(Message);

        }

        private void EscreverTextoNoArquivo(string message)
        {
            string caminhoArquivoLog = @"C:\Users\kaylan\source\Log\Log.txt";

            using (StreamWriter sw = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    sw.WriteLine(message);
                    sw.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
    }
}
