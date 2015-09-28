using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class GlobalSettings
    {
        public string EMailSenderName;
        public AddressUsernamePassword EMailSettings;
        public string EMailPickupDirectory;

        public string WorkFolder;
        public int CheckIntervalMinutes;

        public IEnumerable<MessageReceiver> ResultMessageReceivers;
    }
}