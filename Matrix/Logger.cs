using Microsoft.Extensions.Logging;

namespace Matrix
{
    public static class Logger
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static readonly ILoggerFactory Factory =
            LoggerFactory.Create(builder => { });
#pragma warning restore CA2211 // Non-constant fields should not be visible
    }
}