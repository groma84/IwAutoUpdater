using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.Commands
{
    public class UpdateInterWattDatabase : Command
    {
        private readonly IDatabaseScript _databaseScript;
        private readonly string _connectionString;
        private readonly string _scriptSubfolder;
        private readonly IDirectory _directory;
        private readonly string _fullPathToScriptDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public UpdateInterWattDatabase(string connectionString, string scriptSubfolder, string workFolder, IUpdatePackage package,
            IDirectory directory, IDatabaseScript databaseScript)
        {
            _workFolder = workFolder;
            _package = package;

            _directory = directory;
            _databaseScript = databaseScript;

            _scriptSubfolder = scriptSubfolder;
            _connectionString = connectionString;

            _fullPathToScriptDirectory = Path.Combine(Path.GetFileNameWithoutExtension(Path.Combine(_workFolder, package.Access.GetFilenameOnly())), scriptSubfolder);
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do(CommandResult resultOfPreviousCommand)
        {
            var scriptFiles = LoadScriptsFromFolder();

            throw new NotImplementedException();
        }

        private IEnumerable<DatabaseScript> LoadScriptsFromFolder()
        {
            var scriptFilePaths = _directory.GetFiles(_fullPathToScriptDirectory, "*.DDL");

            var scriptFiles = scriptFilePaths.Select(sfp => _databaseScript.Load(sfp)).OrderBy(sf => sf.Version).ToArray();

            return scriptFiles;
        }
    }
}
