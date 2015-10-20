﻿using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands
{
    public class SendNotifications : Command
    {
        private readonly string _body;
        private readonly string _topic;
        private readonly IEnumerable<INotificationReceiver> _receivers;
        private readonly IUpdatePackage _package;

        public SendNotifications(IEnumerable<INotificationReceiver> receivers, string topic, string body, IUpdatePackage package)
        {
            _package = package;
            _receivers = receivers;
            _topic = topic;
            _body = body;
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
            List<Exception> exceptions = new List<Exception>();
            foreach (var receiver in _receivers)
            {
                try
                {
                    receiver.SendNotification(_topic, _body);

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
            var x = new SendNotifications(_receivers, _topic, _body, _package);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }
    }
}
