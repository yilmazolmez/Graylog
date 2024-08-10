using Graylog.Common.Constants;
using Graylog.Common.LogEvents;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Graylog.API.Filters
{
    public class LogEventActionsFilter : IAsyncActionFilter
    {
        private readonly ILogEvent _logEvent;

        public LogEventActionsFilter(ILogEvent logEvent)
        {
            _logEvent = logEvent;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = (string)context.RouteData.Values["Action"]!;
            var controllerName = (string)context.RouteData.Values["Controller"]!;

            _logEvent.AddOrUpdate(LogKeyConstant.Action, actionName);
            _logEvent.AddOrUpdate(LogKeyConstant.Controller, controllerName);

            Activity.Current?.SetTag(LogKeyConstant.Action, actionName);
            Activity.Current?.SetTag(LogKeyConstant.Controller, controllerName);

            await next.Invoke();
        }
    }
}
