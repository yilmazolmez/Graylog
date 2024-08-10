using Graylog.API.Middlewares.TraceIdentifier;
using Graylog.Common.Constants;
using Graylog.Common.LogEvents;
using System.Diagnostics;
using System.Net;

namespace Graylog.API.Middlewares
{
    public class TraceIdAndLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceIdAndLoggerMiddleware> _logger;

        public TraceIdAndLoggerMiddleware(RequestDelegate next, ILogger<TraceIdAndLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ILogEvent logEvent)
        {
            var startTimeUtc = DateTime.UtcNow;
            DateTime endTime;
            TimeSpan duration;

            try
            {

                var traceId = Activity.Current?.TraceId.ToString() ?? TraceInfo.Info.TraceId;

                context.TraceIdentifier = traceId;

                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("TraceId"))
                    {
                        context.Response.Headers.TryAdd("TraceId", context.TraceIdentifier);
                    }
                    if (!context.Response.Headers.ContainsKey("MachineName"))
                    {
                        context.Response.Headers.TryAdd("MachineName", Environment.MachineName);
                    }

                    return Task.CompletedTask;
                });

                await _next(context);

                endTime = DateTime.UtcNow;
                duration = endTime - startTimeUtc;

                logEvent.AddOrUpdate(LogKeyConstant.TimeSpentOnController, duration);

                if (context.Request.Path == "/health-check") return;

                if (logEvent.HasException())
                    _logger.LogError("{@EventData}", logEvent.Data);
                else
                    _logger.LogInformation("{@EventData}", logEvent.Data);
            }
            catch (Exception ex)
            {
                logEvent.AddException(ex);
                logEvent.AddOrUpdate(LogKeyConstant.HttpStatusCode, context.Response.StatusCode);

                endTime = DateTime.UtcNow;
                duration = endTime - startTimeUtc;

                logEvent.AddOrUpdate(LogKeyConstant.TimeSpentOnController, duration);

                _logger.LogError(ex, "{@EventData}", logEvent.Data);

                var errorMessage = ex.Message;

                var jsonObject = new
                {
                    HttpStatusCode = (int)HttpStatusCode.InternalServerError,
                    IsSuccessful = false,
                    ErrorMessageList = new List<string>() { errorMessage }
                };

                await context.Response.WriteAsJsonAsync(jsonObject);
            }
        }
    }
}
