﻿using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
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
            var autoUpdaterRunner = autoUpdater.CreateAndRunEndlessLoops(settings);

            Task.WaitAll(autoUpdaterRunner);


            logger.Info("IWAU ended");
        }        
    }
}
