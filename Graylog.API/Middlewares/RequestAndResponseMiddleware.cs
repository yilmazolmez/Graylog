using Graylog.Common.Constants;
using Graylog.Common.LogEvents;
using Microsoft.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Graylog.API.Middlewares
{
    public class RequestAndResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestAndResponseMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }


        public async Task InvokeAsync(HttpContext context, ILogEvent logEvent)
        {
            logEvent.AddOrUpdate(LogKeyConstant.HttpMethod, context.Request.Method);
            logEvent.AddOrUpdate(LogKeyConstant.TraceId, context.TraceIdentifier);
            logEvent.AddOrUpdate(LogKeyConstant.Schema, context.Request.Scheme);
            logEvent.AddOrUpdate(LogKeyConstant.Host, context.Request.Host.ToString());
            logEvent.AddOrUpdate(LogKeyConstant.RequestPath, context.Request.Path.ToString());
            logEvent.AddOrUpdate(LogKeyConstant.QueryString, context.Request.QueryString.ToString());
            logEvent.AddOrUpdate(LogKeyConstant.IpAddress, context.Connection.RemoteIpAddress.ToString());

            logEvent.AddOrUpdate(LogKeyConstant.RequestHeaders, JsonConvert.SerializeObject(context.Request.Headers));

            await AddRequestBodyContentToActivityTags(context, logEvent);
            await AddResponseBodyContentToActivityTags(context, logEvent);
        }


        private async Task AddRequestBodyContentToActivityTags(HttpContext context, ILogEvent logEvent)
        {
            context.Request.EnableBuffering();

            var requestBodyStreamReader = new StreamReader(context.Request.Body);

            var requestBodyContent = await requestBodyStreamReader.ReadToEndAsync();

            logEvent.AddOrUpdate(LogKeyConstant.RequestBody, requestBodyContent);

            context.Request.Body.Position = 0;
        }

        private async Task AddResponseBodyContentToActivityTags(HttpContext context, ILogEvent logEvent)
        {
            var originalResponse = context.Response.Body;

            await using var responseBodyMemoryStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = responseBodyMemoryStream;


            await _next.Invoke(context);


            responseBodyMemoryStream.Position = 0;

            var responseBodyStreamReader = new StreamReader(responseBodyMemoryStream);

            var responseBodyContent = await responseBodyStreamReader.ReadToEndAsync();


            logEvent.AddOrUpdate(LogKeyConstant.HttpStatusCode, context.Response.StatusCode.ToString());
            logEvent.AddOrUpdate(LogKeyConstant.ResponseHeaders, JsonConvert.SerializeObject(context.Response.Headers));
            logEvent.AddOrUpdate(LogKeyConstant.ResponseBody, responseBodyContent);

            context.Response.Body.Position = 0;

            await responseBodyMemoryStream.CopyToAsync(originalResponse);
        }
    }
}
