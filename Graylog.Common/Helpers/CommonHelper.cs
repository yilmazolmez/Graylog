using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graylog.Common.Helpers
{
    public static class CommonHelper
    {
        public static string GenerateTraceId()
        {
            var guid = Guid.NewGuid().ToString();
            var now = DateTime.UtcNow;

            return $"{now:yyyy-MM-dd-HH-mm}:{guid}";
        }
    }
}
