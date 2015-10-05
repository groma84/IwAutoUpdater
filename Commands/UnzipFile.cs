using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.Commands
{
    public class UnzipFile : Command
    {
        private readonly string _fullPathToLocalFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public UnzipFile(string workFolder, IUpdatePackage package)
        {
            _workFolder = workFolder;
            _package = package;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());
        }

        public override CommandResult Do(CommandResult lastResult)
        {
            // das lokale Verzeichnis reicht, weil in der zip-Datei sowieso immer noch 
            // der passende Unterordner drin steckt
            var extractTo = Path.GetDirectoryName(_fullPathToLocalFile); 
            System.IO.Compression.ZipFile.ExtractToDirectory(_fullPathToLocalFile, extractTo);

            return new CommandResult(true);
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }
    }
}
