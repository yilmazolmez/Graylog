using Graylog.Common.Constants;
using Microsoft.OpenApi.Models;

namespace Graylog.API.Modules
{
    public static class SwaggerModule
    {
        public static void AddSwaggerModule(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = $"{AppConstants.SolutionName}.Api", Version = "v1" });

                //TO DO : Kontroler edilmeli
                //c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = JwtBearerDefaults.AuthenticationScheme
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = JwtBearerDefaults.AuthenticationScheme
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});

                //c.ExampleFilters();

                //c.OperationFilter<SwaggerJsonIgnoreFilter>();
            });
        }
    }
}
