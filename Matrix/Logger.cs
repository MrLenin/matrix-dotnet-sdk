using Microsoft.Extensions.Logging;

namespace Matrix
{
    public static class Logger
    {
        public static readonly ILoggerFactory Factory =
            LoggerFactory.Create(builder => { });
    }
}