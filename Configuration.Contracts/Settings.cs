using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class Settings
    {
        public GlobalSettings Global;
        public IEnumerable<ServerSettings> Servers;
    }
}
