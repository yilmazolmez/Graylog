using Graylog.Common.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Common.LogEvents
{
    public static class LogEventGlobalConfig
    {
        public static short MaxStringLengthInList { get; set; } = 500;
        public static short MaxStringLength { get; set; } = 2000;
    }
    [LogIgnore]
    public interface ILogEvent
    {
        IReadOnlyDictionary<string, object> Data { get; }
        TimeSpan LogEventDuration { get; }
        bool HasIgnoredLog { get; }
        void AddOrUpdate(string key, object value);
        void AddException(Exception exception);
        bool HasException();
    }

    [LogIgnore]
    public class LogEvent : ILogEvent
    {
        public LogEvent()
        {
            if (Assembly.GetEntryAssembly() != null)
                AddOrUpdate(LogKeyConstant.ProjectName, Assembly.GetEntryAssembly()?.GetName().Name);

            AddOrUpdate(LogKeyConstant.LogType, "LogEvent");
            AddOrUpdate(LogKeyConstant.LogEventCreateTimeUtc, DateTime.UtcNow);
            AddOrUpdate(LogKeyConstant.MachineName, Environment.MachineName);
        }



        private ConcurrentDictionary<string, object> EventData { get; } = new ConcurrentDictionary<string, object>();
        public IReadOnlyDictionary<string, object> Data
        {
            get
            {
                AddOrUpdate(LogKeyConstant.LogEventDuration, LogEventDuration);

                var eventData = new Dictionary<string, object>(EventData);

                return new SortedDictionary<string, object>(eventData);
            }
        }
        public TimeSpan LogEventDuration => DateTime.UtcNow - (DateTime)EventData[LogKeyConstant.LogEventCreateTimeUtc];
        public bool HasIgnoredLog { get; private set; }





        public void AddOrUpdate(string key, object value)
        {
            if (value.IgnoredByLogEvent())
            {
                EventData[key] = "Log ignored";
                HasIgnoredLog = true;
            }
            else
            {
                switch (value)
                {
                    case TimeSpan t:
                        AddOrUpdate($"{key}Seconds", t.TotalSeconds);
                        return;
                    case string v:
                        {
                            var length = v.Length > LogEventGlobalConfig.MaxStringLength
                                ? LogEventGlobalConfig.MaxStringLength
                                : v.Length;
                            value = v.Substring(0, length);
                            break;
                        }
                }

                EventData[key] = value;
            }
        }
        public void AddException(Exception exception)
        {
            if (exception == null) return;

            AddOrUpdate(LogKeyConstant.Exception, exception.ToString());
            AddOrUpdate(LogKeyConstant.ExceptionType, exception.GetType().FullName);
        }
        public bool HasException()
        {
            return EventData.ContainsKey(LogKeyConstant.Exception);
        }
    }
}
