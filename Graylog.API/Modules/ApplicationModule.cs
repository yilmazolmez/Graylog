using Graylog.Application;

namespace Graylog.API.Modules
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IGraylogService, GraylogService>();
        }
    }
}
