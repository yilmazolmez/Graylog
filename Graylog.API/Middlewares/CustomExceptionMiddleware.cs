using Graylog.Contracts.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Net.Mime;
using System.Net;

namespace Graylog.API.Middlewares
{
    public static class CustomExceptionMiddleware
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    string errorMessage = string.Empty;
                    HttpStatusCode statusCode;

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;


                    if (exceptionFeature?.Error is ArgumentNullException ex2)
                    {
                        errorMessage = ex2.Message;
                        statusCode = HttpStatusCode.BadRequest;
                    }
                    else if (exceptionFeature?.Error is NotFoundException ex)
                    {
                        errorMessage = ex.Message;
                        statusCode = HttpStatusCode.NotFound;
                    }
                    else
                    {
                        errorMessage = "Default CustomExceptionMiddleware Error Message";
                        statusCode = HttpStatusCode.InternalServerError;
                    }

                    var jsonObject = new
                    {
                        HttpStatusCode = (int)statusCode,
                        IsSuccessful = false,
                        ErrorMessageList = new List<string>() { errorMessage }
                    };

                    await context.Response.WriteAsJsonAsync(jsonObject);
                });
            });
        }
    }
}
