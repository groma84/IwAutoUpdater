using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                while (true)
                {
                    OneWorkLoop();
                    Thread.Sleep(15000);
                }
            });

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>internal für Zugriff aus Tests</remarks>
        internal void OneWorkLoop()
        {
            Command command;

            var keysToRemove = new List<string>(CommandsProducerConsumer.Queues.Count);
            foreach (var queue in CommandsProducerConsumer.Queues)
            {
                var commandQueue = queue.Value;

                CommandResult lastResult = new CommandResult(true); // wenn wir es nicht mit true vorbelegen kann durch die verUNDung nie true rauskommen                                
                while (!commandQueue.IsCompleted)
                {
                    command = commandQueue.Take(); // Take() blockiert, solange nichts abzurufen ist

                    if (command != null)
                    {
                        var newResult = ExecuteOneCommand(command, lastResult, queue, CommandsProducerConsumer.Queues);

                        lastResult = new CommandResult()
                        {
                            Successful = lastResult.Successful && newResult.Successful,
                            PreviousErrors = lastResult.PreviousErrors.Concat(newResult.ErrorsInThisCommand)
                        };

                        command = null;
                    }
                }

                keysToRemove.Add(queue.Key);
            }

            foreach (var key in keysToRemove)
            {
                BlockingCollection<Command> unused;
                CommandsProducerConsumer.Queues.TryRemove(key, out unused);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandQueue"></param>
        /// <remarks>internal nur, um in den Tests ranzukommen</remarks>
        internal CommandResult ExecuteOneCommand(Command command, CommandResult lastResult, KeyValuePair<string, BlockingCollection<Command>> queue, ConcurrentDictionary<string, BlockingCollection<Command>> queues)
        {
            _logger.Info("Starting Command {Command}", command);
            var result = command.Do(lastResult);
            _logger.Info("Finished Command {Command}", command);

            if (result.Successful && command.RunAfterCompletedWithResultTrue != null)
            {
                queue.Value.Add(command.RunAfterCompletedWithResultTrue);
            }
            else if (!result.Successful && command.RunAfterCompletedWithResultFalse != null)
            {
                queue.Value.Add(command.RunAfterCompletedWithResultFalse);
            }

            if (queue.Value.Count == 0)
            {
                _logger.Info("Finished Working Queue for Command {Command}", command);
                queue.Value.CompleteAdding();
            }

            return result;
        }
    }
}
