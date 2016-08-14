using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IwAutoUpdater.Service
{
    public class IwAutoUpdaterService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IAutoUpdaterThreadFactory _autoUpdaterThreadFactory;

        public IwAutoUpdaterService(IAutoUpdaterThreadFactory autoUpdaterThreadFactory, ILogger logger, IConfiguration configuration)
        {
            _autoUpdaterThreadFactory = autoUpdaterThreadFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public void Start()
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        _logger.Info("IwAutoUpdaterService starting");

                        var settings = _configuration.Get("config.json");
                        var autoUpdaterTask = _autoUpdaterThreadFactory.CreateAndRunEndlessLoops(settings);
                        autoUpdaterTask.Start();
                        Task.WaitAll(new[] { autoUpdaterTask });
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("IwAutoUpdaterService exception: {@Exception}", ex);
                    }

                    Thread.Sleep(300000);
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning).Start();

        }

        public void Stop()
        {
            _logger.Info("IwAutoUpdaterService stopping");
        }
    }
}
