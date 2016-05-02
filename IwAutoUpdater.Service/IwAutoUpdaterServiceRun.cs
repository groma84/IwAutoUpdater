using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace SuperVisor.Service
{
    public partial class IwAutoUpdaterServiceRun : ServiceBase
    {
        public IwAutoUpdaterServiceRun()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var t = new Thread(new ThreadStart(() =>
            {
                var container = IwAutoUpdater.DIMappings.Container.Init();

                var logger = container.GetInstance<ILogger>();
                try
                {
                    var configuration = container.GetInstance<IConfiguration>();
                    var autoUpdater = container.GetInstance<IAutoUpdaterThreadFactory>();

                    logger.Info("IwAutoUpdaterServiceRun started");

                    var settings = configuration.Get(args[0]);
                    var autoUpdaterRunner = autoUpdater.CreateAndRunEndlessLoops(settings);

                    Task.WaitAll(autoUpdaterRunner);
                }
                catch (Exception ex)
                {
                    logger.Error("IwAutoUpdaterServiceRun->OnStart" + ex.Message + " @ " + ex.StackTrace);
                }

            }));
            t.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
