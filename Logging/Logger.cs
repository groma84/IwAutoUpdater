using Serilog;
using System;

namespace IwAutoUpdater.CrossCutting.Logging
{
    public class Logger : Contracts.ILogger
    {
        Serilog.ILogger _log;

        public Logger()
        {
            _log = new LoggerConfiguration()
                        .MinimumLevel.Debug()
#if DEBUG
                        .WriteTo.ColoredConsole()
#endif
                        .WriteTo.RollingFile($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\IwAutoUpdaterService\Logs\" + "log_{Date}.txt", retainedFileCountLimit: 10)
#if (!DEBUG)
                        // unsere EventSource wird schon von TopShelf angelegt (heißt automatisch so wie der Service)
                        .WriteTo.EventLog("IwAutoUpdaterService", "IwAutoUpdaterService", manageEventSource: false, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
#endif
                        .CreateLogger();
        }

        void Contracts.ILogger.Info(string message)
        {
            _log.Information(message);
        }

        void Contracts.ILogger.Info(string messageTemplate, params object[] data)
        {
            _log.Information(messageTemplate, data);
        }

        void Contracts.ILogger.Debug(string message)
        {
            _log.Debug(message);
        }

        void Contracts.ILogger.Debug(string messageTemplate, params object[] data)
        {
            _log.Debug(messageTemplate, data);
        }

        void Contracts.ILogger.Error(string message)
        {
            _log.Error(message);
        }

        void Contracts.ILogger.Error(string messageTemplate, params object[] data)
        {
            _log.Error(messageTemplate, data);
        }
    }
}
