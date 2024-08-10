using Graylog.Common.Helpers;

namespace Graylog.API.Middlewares.TraceIdentifier
{
    public class TraceInfo : IDisposable
    {
        private static readonly AsyncLocal<TraceInfo> _info = new AsyncLocal<TraceInfo>();

        public string? TraceId { get; set; }


        public static TraceInfo Info { get => _info.Value ?? New; set => _info.Value = value; }

        private static TraceInfo New
        {
            get
            {
                _info.Value = new TraceInfo
                {
                    TraceId = CommonHelper.GenerateTraceId()
                };

                return _info.Value;
            }
        }

        public void Dispose()
        {
        }
    }
}
