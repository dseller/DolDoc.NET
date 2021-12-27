using Microsoft.Extensions.Logging;

namespace DolDoc.Editor.Core
{
    public static class LogSingleton
    {
        public static ILoggerFactory Instance { get; private set; }

        internal static void Initialize(ILoggerFactory loggerFactory)
        {
            Instance = loggerFactory;
        }
    }
}
