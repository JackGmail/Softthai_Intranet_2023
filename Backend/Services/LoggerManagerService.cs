//using NLog;
using Backend.Interfaces;

namespace Backend.Service
{
    public class LoggerManagerService : ILoggerManager
    {
        //private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();
        //public void LogDebug(string message) => logger?.Debug(message);
        //public void LogError(string message) => logger?.Error(message);
        //public void LogInfo(string message) => logger?.Info(message);
        //public void LogWarning(string message) => logger?.Warn(message);
        public void LogDebug(string message) { }
        public void LogError(string message) { }
        public void LogInfo(string message) { }
        public void LogWarning(string message) { }

    }
}
