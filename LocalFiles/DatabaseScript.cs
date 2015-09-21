using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles
{
    public class DatabaseScript : IDatabaseScript
    {
        Regex _versionRegex = new Regex(@"^\D*(?<version>\d*).*", RegexOptions.IgnoreCase);

        Contracts.DatabaseScript IDatabaseScript.Load(string filePath)
        {
            var version = int.Parse(_versionRegex.Match(filePath).Groups["version"].Value);            
            var lines = File.ReadAllLines(filePath, Encoding.Default);

            return new Contracts.DatabaseScript()
            {
                Version = version,
                Lines = lines
            };
        }
    }
}
