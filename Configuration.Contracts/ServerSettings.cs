using IwAutoUpdater.CrossCutting.Base;

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

        public string DatabaseUpdateConnectionString;
        public string DatabaseScriptSubfolder;
    }
}