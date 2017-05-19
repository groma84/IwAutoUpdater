using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IWAutoUpdater.CrossCutting.SFW.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IwAutoUpdater.BLL.Commands
{
    public class SendNotifications : Command
    {
        private readonly bool _withSkipDatabaseUpdate;
        private readonly bool _isDownloadOnly;

        private readonly INowGetter _nowGetter;
        private readonly IBlackboard _blackboard;
        private readonly IEnumerable<INotificationReceiver> _receivers;
        private readonly IUpdatePackage _package;

        public SendNotifications(IEnumerable<INotificationReceiver> receivers, bool downloadOnly, bool withSkipDatabaseUpdate, IUpdatePackage package, INowGetter nowGetter, IBlackboard blackboard)
        {
            _package = package;
            _receivers = receivers;
            _blackboard = blackboard;
            _nowGetter = nowGetter;
            _withSkipDatabaseUpdate = withSkipDatabaseUpdate;
            _isDownloadOnly = downloadOnly;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do()
        {
            var text = BuildNotificationText(_package);

            List<Exception> exceptions = new List<Exception>();
            foreach (var receiver in _receivers)
            {
                try
                {
                    receiver.SendNotification(text.Subject, text.Message);
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

        public override Command Copy()
        {
            var x = new SendNotifications(_receivers, _isDownloadOnly, _withSkipDatabaseUpdate, _package, _nowGetter, _blackboard);
            x.RunAfterCompletedWithResultFalse = RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(ResultsOfPreviousCommands);

            return x;
        }

        private class NotificationText
        {
            public string Subject;
            public string Message;
        }

        private NotificationText BuildNotificationText(IUpdatePackage package)
        {
            var shortPackageName = GetShortPackageName(package.PackageName);

            var message = BuildMessage(package.PackageName);

            var verb = _isDownloadOnly ? "heruntergeladen" : "aktualisiert";

            return new NotificationText()
            {
                Subject = $"Paket '{shortPackageName}' wurde um {_nowGetter.Now} {verb}",
                Message = message
            };
        }

        private string BuildMessage(string packageName)
        {
            var verb = _isDownloadOnly ? "heruntergeladen" : "aktualisiert";

            var sb = new StringBuilder();
            sb.AppendLine($"Paket '{packageName}' wurde um {_nowGetter.Now} automatisch {verb}.");
            sb.AppendLine();
            if (_withSkipDatabaseUpdate)
            {
                sb.AppendLine($"Das Datenbankupdate wurde planmäßig übersprungen.");
                sb.AppendLine();
            }

            var blackboardEntries = _blackboard.Get(packageName);
            if (blackboardEntries.Count() > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Weitere Hinweise: ");
                foreach (var blackboardEntry in blackboardEntries)
                {
                    sb.AppendLine(blackboardEntry.Content.ToString());
                }
            }
            return sb.ToString();
        }

        private static string GetShortPackageName(string packageName)
        {
            var splitByBackslash = packageName.Split(new[] { '\\' });
            var splitBySlash = packageName.Split(new[] { '/' });

            if (splitByBackslash.Length > 1)
            {
                return splitByBackslash.Last();
            }

            if (splitBySlash.Length > 1)
            {
                return splitBySlash.Last();
            }

            return packageName;
        }
    }
}
