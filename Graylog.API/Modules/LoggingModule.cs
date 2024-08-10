using NLog.Extensions.Logging;

namespace Graylog.API.Modules
{
    public static class LoggingModule
    {
        public static void AddLoggingModule(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                loggingBuilder.AddNLog();
            });
        }
    }
}
