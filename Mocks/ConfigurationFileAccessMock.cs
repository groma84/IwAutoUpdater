using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocks
{
    public class ConfigurationFileAccessMock : IConfigurationFileAccess
    {
        public Dictionary<string, string> ReadAllText = new Dictionary<string, string>();
        string IConfigurationFileAccess.ReadAllText(string path)
        {
            return ReadAllText[path];
        }
    }
}
