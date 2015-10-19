using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.Commands
{
    public class SendErrorNotifications : Command
    {
        private readonly Command _failedCommand;
        private readonly IEnumerable<INotificationReceiver> _receivers;

        public SendErrorNotifications(IEnumerable<INotificationReceiver> receivers, Command failedCommand)
        {
            _receivers = receivers;
            _failedCommand = failedCommand;
        }

        public override string PackageName
        {
            get
            {
                return _failedCommand.PackageName;
            }
        }

        public override CommandResult Do(CommandResult resultOfPreviousCommand)
        {
            var topic = BuildTopic(_failedCommand);
            var body = BuildBody(_failedCommand, resultOfPreviousCommand);

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

        private string BuildBody(Command failedCommand, CommandResult resultOfPreviousCommand)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"IWAutoUpdater: Command {failedCommand.GetType().Name} for Package {failedCommand.PackageName} failed with error: ");
            foreach (var error in resultOfPreviousCommand.PreviousErrors.Concat(resultOfPreviousCommand.ErrorsInThisCommand))
            {
                sb.AppendLine(error.Exception + " -- " + error.Text);
            }

            return sb.ToString();
        }

        private string BuildTopic(Command failedCommand)
        {
            return $"IWAutoUpdater: Command {failedCommand.GetType().Name} for Package {failedCommand.PackageName} failed";
        }
    }
}
