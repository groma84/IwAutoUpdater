using IwAutoUpdater.CrossCutting.Base;
using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class ServerSettings
    {
        public GetDataMethod Type;
        public string Path;
        public string HttpDownloadUsername;
        public string HttpDownloadPassword;

        public bool DownloadOnly;
        public bool SkipDatabaseUpdate;

        public string PreInstallCommand;
        public string PreInstallCommandArguments;
        public string InstallerCommand;
        public string InstallerCommandArguments;
        public string PostInstallCommand;
        public string PostInstallCommandArguments;

        public string DatabaseUpdaterCommand;
        public string DatabaseUpdaterCommandArguments;
        public string ConnectionString;

        public IEnumerable<string> CheckUrlsAfterInstallation;
        public AddressUsernamePassword CheckUrlProxySettings;

        public string ReadVersionInfoFrom;

        public string ZipPassword;
    }
}