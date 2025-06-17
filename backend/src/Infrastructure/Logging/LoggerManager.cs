using Application.Interfaces;
using Serilog;

namespace Infrastructure.Logging;

public class LoggerManager : ILoggerManager
{
    public void LogInfo(string message) => Log.Information(message);
    public void LogWarn(string message) => Log.Warning(message);
    public void LogError(string message) => Log.Error(message);
    public void LogDebug(string message) => Log.Debug(message);
}