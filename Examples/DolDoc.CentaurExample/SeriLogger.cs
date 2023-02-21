using Serilog;
using ILogger = DolDoc.Shared.ILogger;

namespace DolDoc.CentaurExample
{
    public class SeriLogger : ILogger
    {
        private readonly Serilog.ILogger logger;

        public SeriLogger(Serilog.ILogger logger)
        {
            this.logger = logger;
        }

        public void Debug(string message, params object[] parameters) => logger.Debug(message, parameters);

        public void Info(string message, params object[] parameters) => logger.Information(message, parameters);

        public void Warning(string message, params object[] parameters) => logger.Warning(message, parameters);

        public void Error(string message, params object[] parameters) => logger.Error(message, parameters);
        public void Enter(string text)
        {
        }

        public void Leave()
        {
        }
    }
}