using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System.IO;

namespace IwAutoUpdater.CrossCutting.Configuration
{
    public class ConfigurationFileAccess : IConfigurationFileAccess
    {
        string IConfigurationFileAccess.ReadAllText(string path)
        {
            return File.ReadAllText(path);

        }
    }
}
