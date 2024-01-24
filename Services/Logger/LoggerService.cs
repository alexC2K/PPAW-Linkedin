namespace Linkedin.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public void LogError(string error)
        {
            Console.WriteLine($"[ERROR]: {error}");
        }

        public void LogMessage(string message)
        {
            Console.WriteLine($"[INFO]: {message}");
        }

        public void LogWarning(string warning)
        {
            Console.WriteLine($"[WARNING]: {warning}");
        }
    }
}
