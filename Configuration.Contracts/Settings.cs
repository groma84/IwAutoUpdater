using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class Settings
    {
        public GlobalSettings Global;
        public IEnumerable<ServerSettings> Servers;
    }
}
