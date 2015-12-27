using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using SFW.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IwAutoUpdater.BLL.Commands
{
    public class SendErrorNotifications : Command
    {
        private readonly IBlackboard _blackboard;
        private readonly Command _failedCommand;
        private readonly IEnumerable<INotificationReceiver> _receivers;

        public SendErrorNotifications(IEnumerable<INotificationReceiver> receivers, Command failedCommand, IBlackboard blackboard)
        {
            _receivers = receivers;
            _failedCommand = failedCommand;
            _blackboard = blackboard;
        }

        public override string PackageName
        {
            get
            {
                return _failedCommand.PackageName;
            }
        }

        public override Command Copy()
        {
            var x = new SendErrorNotifications(_receivers, _failedCommand, _blackboard);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }

        public override CommandResult Do()
        {
            var topic = BuildTopic(_failedCommand);
            var body = BuildBody(_failedCommand, ResultsOfPreviousCommands);

            List<Exception> exceptions = new List<Exception>();
            foreach (var receiver in _receivers)
            {
                try
                {
                    receiver.SendNotification(topic, body);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                var asErrors = exceptions.Select(ex => new Error() { Exception = ex, Text = "Unerwarteter Ausnahmefehler in SendNotifications" });
                return new CommandResult(false, asErrors);
            }

            return new CommandResult(true);
        }

        private string BuildBody(Command failedCommand, IEnumerable<CommandResult> resultsOfPreviousCommands)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"IWAutoUpdater: Command {failedCommand.GetType().Name} for Package {failedCommand.PackageName} failed with error: ");
            foreach (var result in resultsOfPreviousCommands)
            {
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Exception + " -- " + error.Text);
                }
            }

            var blackboardEntries = _blackboard.Get(failedCommand.PackageName);
            if (blackboardEntries.Count() > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Blackboard entries: ");
                foreach (var blackboardEntry in blackboardEntries)
                {
                    sb.AppendLine(blackboardEntry.Content.ToString());
                }
            }

            return sb.ToString();
        }

        private string BuildTopic(Command failedCommand)
        {
            return $"IWAutoUpdater: Command {failedCommand.GetType().Name} for Package {failedCommand.PackageName} failed";
        }
    }
}
