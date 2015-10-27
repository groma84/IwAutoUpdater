using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWAU.Console
{
    class IWAU
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                System.Console.WriteLine("Es muss genau ein Parameter angegeben werden: Der Pfad zur config.json");
                return;
            }

            var container = IwAutoUpdater.DIMappings.Container.Init();

            var logger = container.GetInstance<ILogger>();
            var configuration = container.GetInstance<IConfiguration>();
            var autoUpdater = container.GetInstance<IAutoUpdaterThreadFactory>();            

            logger.Info("IWAU started");

            var settings = configuration.Get(args[0]);
            autoUpdater.CreateAndRunEndlessLoops(settings); // blockiert
            
            logger.Info("IWAU ended");
        }
    }
}
