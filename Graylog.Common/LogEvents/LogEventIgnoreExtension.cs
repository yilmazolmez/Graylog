using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Common.LogEvents
{
    public static class LogEventIgnoreExtension
    {
        public static bool IgnoredByLogEvent(this object instance)
        {
            return instance?.GetType().GetCustomAttributes()
                .Any(attr => attr.GetType() == typeof(LogIgnoreAttribute)) ?? false;
        }

        public static bool IgnoredByLogEvent(this PropertyInfo property)
        {
            return (property?.GetCustomAttributes()
                .Any(attr => attr.GetType() == typeof(LogIgnoreAttribute)) ?? false) ||
                   (property?.PropertyType.GetCustomAttributes()
                .Any(attr => attr.GetType() == typeof(LogIgnoreAttribute)) ?? false);
        }
    }
}
