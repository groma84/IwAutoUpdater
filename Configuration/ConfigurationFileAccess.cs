using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
