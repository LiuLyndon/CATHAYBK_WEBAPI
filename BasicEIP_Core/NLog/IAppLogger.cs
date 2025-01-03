namespace BasicEIP_Core.NLog
{
    public interface IAppLogger<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogDebug(string message);
        void LogCritical(string message);
    }
}