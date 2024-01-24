namespace Linkedin.Services.Logger
{
    public interface ILoggerService
    {
        /// <summary>
        /// Logs a normal message.
        /// </summary>
        void LogMessage(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        void LogWarning(string warning);

        /// <summary>
        /// Logs an error.
        /// </summary>
        void LogError(string error);
    }
}
