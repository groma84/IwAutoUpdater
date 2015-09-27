using IwAutoUpdater.CrossCutting.Base;
using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class ServerSettings
    {
        public GetDataMethod Type;
        public string Path;

        public bool DownloadOnly;
        public bool SkipDatabaseUpdate;

        public string InstallerCommand;
        public string InstallerCommandArguments;

        public string DatabaseUpdaterCommand;
        public string DatabaseUpdaterCommandArguments;
        public string ConnectionString;

        public IEnumerable<string> CheckUrlsAfterInstallation;
        public string CheckUrlProxyAddress;
        public string CheckUrlProxyUsername;
        public string CheckUrlProxyPassword;
    }
}