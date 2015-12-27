using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System.Threading;
using IwAutoUpdater.CrossCutting.Logging.Contracts;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace IwAutoUpdater.BLL.AutoUpdater
{
    public class AutoUpdaterThreadFactory : IAutoUpdaterThreadFactory
    {
        private readonly IAutoUpdaterCommandCreator _autoUpdaterCommandCreator;
        private readonly IAutoUpdaterWorker _autoUpdaterWorker;
        private readonly ILogger _logger;

        public AutoUpdaterThreadFactory(IAutoUpdaterCommandCreator autoUpdaterCommandCreator, IAutoUpdaterWorker autoUpdaterWorker, ILogger logger)
        {
            _logger = logger;
            _autoUpdaterWorker = autoUpdaterWorker;
            _autoUpdaterCommandCreator = autoUpdaterCommandCreator;
        }

        async Task IAutoUpdaterThreadFactory.CreateAndRunEndlessLoops(Settings settings)
        {
            var t1 = CommandCreatorRun(_autoUpdaterCommandCreator, settings, _logger);
            var t2 = CommandWorkerRun(_autoUpdaterWorker, _logger);
            
            Task.WaitAll(t1, t2);
        }

        private async Task CommandCreatorRun(IAutoUpdaterCommandCreator commandCreator, Settings settings, ILogger logger)
        {
            while (true)
            {
                try
                {
                    await commandCreator.NeverendingCreationLoop(settings);

                }
                catch (Exception ex)
                {
                    logger.Error("Error in {Methodname}: {Exception}", "CommandCreatorRun", ex);
                    Thread.Sleep(60000);
                }
            }
        }

        private async Task CommandWorkerRun(IAutoUpdaterWorker commandWorker, ILogger logger)
        {
            while (true)
            {
                try
                {
                    await commandWorker.NeverendingWorkLoop();
                }
                catch (Exception ex)
                {
                    logger.Error("Error in {Methodname}: {Exception}", "CommandCreatorRun", ex);
                    Thread.Sleep(60000);
                }
            }
        }
    }
}
