using Microsoft.Extensions.Logging;

namespace BasicEIP_Core.NLog
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message) => _logger.LogInformation(message);
        public void LogWarning(string message) => _logger.LogWarning(message);
        public void LogError(string message) => _logger.LogError(message);
        public void LogDebug(string message) => _logger.LogDebug(message);
        public void LogCritical(string message) => _logger.LogCritical(message);
    }
}