using PlatformPOC.PlatformContracts;
using Serilog.Core;
using System;

namespace PlatformPOC.PlatformImplementation
{
    public class PlatformLogger : IPlatformLogger
    {
        Logger _logger;
        string _className;

        public static PlatformLogger CreateLogger(Type type, Logger logger)
        {
            PlatformLogger platFormLogger = new PlatformLogger(type, logger);

            return platFormLogger;
        }        

        private PlatformLogger(Type className, Logger logger)
        {
            _logger = logger;
            _className = className.Name.ToString();
        }

        public void LogDebug(string message)
        {
            _logger.Write(Serilog.Events.LogEventLevel.Debug, message);
        }

        public void LogError(string message)
        {
            _logger.Write(Serilog.Events.LogEventLevel.Error, $"[{_className}] {message}");
        }

        public void LogFatal(string message)
        {
            _logger.Write(Serilog.Events.LogEventLevel.Fatal, $"[{_className}] {message}");
        }

        public void LogInformation(string message)
        {
            _logger.Write(Serilog.Events.LogEventLevel.Information, $"[{_className}] {message}");
        }

        public void LogWarning(string message)
        {
            _logger.Write(Serilog.Events.LogEventLevel.Warning, $"[{_className}] {message}");
        }
    }
}
