using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.CrossCutting.SFW.Contracts;
using System.Threading;
using IwAutoUpdater.CrossCutting.Base;
using System.Collections.Concurrent;

namespace IwAutoUpdater.BLL.AutoUpdater
{
    public class AutoUpdaterCommandCreator : IAutoUpdaterCommandCreator
    {
        private readonly INowGetter _nowGetter;
        private readonly ICommandBuilder _commandBuilder;
        private readonly IConfigurationConverter _configurationConverter;
        private readonly ICheckTimer _checkTimer;

        public AutoUpdaterCommandCreator(ICheckTimer checkTimer, IConfigurationConverter configurationConverter, ICommandBuilder commandBuilder, INowGetter nowGetter)
        {
            _checkTimer = checkTimer;
            _configurationConverter = configurationConverter;
            _commandBuilder = commandBuilder;
            _nowGetter = nowGetter;
        }

        Task IAutoUpdaterCommandCreator.NeverendingCreationLoop(Settings settings)
        {
            var servers = _configurationConverter.ConvertServers(settings.Servers);
            var messageReceivers = _configurationConverter.ConvertMessageReceivers(settings.Global.ResultMessageReceivers, settings.Global.EMailSettings);

            var t = new Task(() =>
            {
                TimeSpan waitTime;

                while (true)
                {
                    CheckIfUpdateIsNecessaryAndEnqueueCommands(settings, servers, messageReceivers);
                    waitTime = CalculateWaitTimeToNextMinute(_nowGetter);
                    Thread.Sleep(waitTime);
                }
            }, TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent);

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nowGetter"></param>
        /// <remarks>internal nur, damit wir in den Tests rankommen</remarks>
        internal TimeSpan CalculateWaitTimeToNextMinute(INowGetter nowGetter)
        {
            return new TimeSpan(0, 0, 60 - nowGetter.Now.Second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="servers"></param>
        /// <param name="messageReceivers"></param>
        /// <remarks>internal nur, damit wir in den Tests rankommen</remarks>
        internal void CheckIfUpdateIsNecessaryAndEnqueueCommands(Settings settings, IEnumerable<IUpdatePackage> servers, IEnumerable<INotificationReceiver> messageReceivers)
        {
            if (_checkTimer.IsCheckForUpdatesNecessary(settings.Global.CheckIntervalMinutes))
            {
                var commands = _commandBuilder.GetCommands(settings.Global.WorkFolder, servers, messageReceivers);

                foreach (var command in commands)
                {
                    // Damit wir keine doppelte Arbeit machen, packen wir Commands nur dann rein, wenn noch kein Command für das gleiche
                    // Update-Paket vorhanden ist.
                    // Das klappt hier nur so einfach, weil wir ja genau genommen immer nur das CheckIfNewer-Command bekommen, und alles weitere 
                    // erst nach und nach in die Warteschlange kommt. 
                    if (!UpdatePackageInCommandAlreadyQueued(command, CommandsProducerConsumer.Queues))
                    {
                        CommandsProducerConsumer.Queues.TryAdd(command.PackageName, new BlockingCollection<Command>());
                        CommandsProducerConsumer.Queues[command.PackageName].Add(command);
                    }
                }
            }
        }

        private bool UpdatePackageInCommandAlreadyQueued(Command command, ConcurrentDictionary<string, BlockingCollection<Command>> commandQueues)
        {
            return commandQueues.ContainsKey(command.PackageName);
        }
    }
}
