using Graylog.Common.LogEvents;

namespace Graylog.API.Modules
{
    public static class CommonModule
    {
        public static void AddCommonModule(this IServiceCollection services)
        {
            services.AddScoped<ILogEvent, LogEvent>();
        }
    }
}
