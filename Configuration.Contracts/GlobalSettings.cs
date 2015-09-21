using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class GlobalSettings
    {
        public EMailSettings EMailSettings;
        public string WorkFolder;
        public int CheckIntervalMinutes;
        public IEnumerable<MessageReceiver> ResultMessageReceivers;
    }
}