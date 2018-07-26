using Kasanova.Common.ApplicationCore.Interfaces;
using System.Diagnostics;

namespace Infrastructure.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {

        public LoggerAdapter()
        {
        }

        public void LogInformation(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }
    }
}
