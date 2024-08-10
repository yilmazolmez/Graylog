using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Common.Constants
{
    public static class LogKeyConstant
    {
        public const string LogType = "logType";
        public const string ProjectName = "projectName";
        public const string LogEventCreateTimeUtc = "logEventCreateTimeUtc";
        public const string LogEventDuration = "logEventDuration";
        public const string MachineName = "machineName";
        public const string Exception = "exception";
        public const string ExceptionType = "exceptionType";
        public const string TimeSpentOnController = "timeSpentOnController";
        public const string HttpStatusCode = "httpStatusCode";
        public const string HttpMethod = "httpMethod";
        public const string TraceId = "traceId";
        public const string Schema = "schema";
        public const string Host = "host";
        public const string RequestPath = "requestPath";
        public const string QueryString = "queryString";
        public const string IpAddress = "ipAddress";
        public const string RequestHeaders = "requestHeaders";
        public const string RequestBody = "requestBody";
        public const string ResponseBody = "responseBody";
        public const string ResponseHeaders = "responseHeaders";
        public const string Action = "action";
        public const string Controller = "controller";
    }
}
