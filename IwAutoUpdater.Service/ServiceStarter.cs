using Topshelf;

namespace IwAutoUpdater.Service
{
    class ServiceStarter
    {
        static void Main(string[] args)
        {
            HostFactory.New(winService =>
            {
                var di = DIMappings.Container.Init();

                winService.Service<IwAutoUpdaterService>(service =>
                {
                    service.ConstructUsing(name => di.GetInstance<IwAutoUpdaterService>());
                    service.WhenStarted(bts => bts.Start());
                    service.WhenStopped(bts => bts.Stop());
                });
                winService.RunAsPrompt();

                winService.SetDescription("InterWatt AutoUpdater Service - installiert heruntergeladene InterWatt-Installationspakete");
                winService.SetDisplayName("InterWatt AutoUpdater Service");
                winService.SetServiceName("IwAutoUpdaterService");

                winService.StartAutomatically();

                winService.EnableServiceRecovery(r =>
                {
                    r.RestartService(5);
                    r.RestartService(10);
                    r.RestartService(20);

                    //should this be true for crashed or non-zero exits
                    r.OnCrashOnly();

                    //number of days until the error count resets
                    r.SetResetPeriod(1);
                });
            }).Run();
        }
    }
}
