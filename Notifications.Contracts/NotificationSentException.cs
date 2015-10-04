using System;

namespace IwAutoUpdater.DAL.Notifications.Contracts
{
    public class NotificationSentException : Exception
    {
        public NotificationSentException(string message) : base(message)
        {
        }

        public NotificationSentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
