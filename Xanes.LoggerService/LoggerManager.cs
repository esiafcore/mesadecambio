using NLog;

namespace Xanes.LoggerService;
public class LoggerManager : ILoggerManager
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {

    }
    public void LogInfo(string message) => _logger.Info(message);

    public void LogWarn(string message) => _logger.Warn(message);

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);
}
