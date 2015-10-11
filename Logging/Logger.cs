using IwAutoUpdater.CrossCutting.Logging.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Logging
{
    public class Logger : Contracts.ILogger
    {
        Serilog.ILogger _log;

        public Logger()
        {
            _log = new LoggerConfiguration()
                        .WriteTo.ColoredConsole()
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
    }
}
