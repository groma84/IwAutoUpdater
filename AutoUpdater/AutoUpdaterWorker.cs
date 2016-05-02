using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater
{
    public class AutoUpdaterWorker : IAutoUpdaterWorker
    {
        private readonly ILogger _logger;

        public AutoUpdaterWorker(ILogger logger)
        {
            _logger = logger;
        }

        Task IAutoUpdaterWorker.NeverendingWorkLoop()
        {
            var t = new Task(() =>
            {
                Command command;

                var commandQueue = CommandsProducerConsumer.Queue;

                while (!commandQueue.IsCompleted) // wir setzen IsCompleted niemals auf true, daher ist das hier (absichtlich) eine Endlosschleife
                {
                    try
                    {
                        command = commandQueue.Take(); // Take() blockiert, solange nichts abzurufen ist

                        if (command != null)
                        {
                            ExecuteOneCommand(command, commandQueue);
                            command = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("NeverendingWorkLoop: " + ex.Message + " @ " + ex.StackTrace);
                    }
                }
            });

            t.ContinueWith((tResult) =>
            {
                if (tResult.Exception != null)
                {
                    var flattened = tResult.Exception.Flatten();
                    _logger.Debug("Exception in AutoUpdaterWorker: {Exception}", flattened);
                    throw flattened;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            t.Start();
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandQueue"></param>
        /// <remarks>internal nur, um in den Tests ranzukommen</remarks>
        internal CommandResult ExecuteOneCommand(Command command, BlockingCollection<Command> queue)
        {
            _logger.Info("Starting Command {Command} for package {Package}", command, command.PackageName);
            var result = command.Do();
            _logger.Debug("Finished Command {Command} with result: {Result}", command, result);

            if (result.Successful && command.RunAfterCompletedWithResultTrue != null)
            {
                command.RunAfterCompletedWithResultTrue.AddResultsOfPreviousCommands(command.ResultsOfPreviousCommands.Concat(new[] { result }));
                queue.Add(command.RunAfterCompletedWithResultTrue);
            }
            else if (!result.Successful && command.RunAfterCompletedWithResultFalse != null)
            {
                command.RunAfterCompletedWithResultFalse.AddResultsOfPreviousCommands(command.ResultsOfPreviousCommands.Concat(new[] { result }));
                queue.Add(command.RunAfterCompletedWithResultFalse);
            }

            return result;
        }
    }
}
