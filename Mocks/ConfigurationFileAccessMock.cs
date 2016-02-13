using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System.Collections.Generic;

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
