using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;
using System;

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

        public override CommandResult Do()
        {
            // das lokale Verzeichnis reicht, weil in der zip-Datei sowieso immer noch 
            // der passende Unterordner drin steckt
            var extractTo = Path.GetDirectoryName(_fullPathToLocalFile); 
            System.IO.Compression.ZipFile.ExtractToDirectory(_fullPathToLocalFile, extractTo);

            return new CommandResult(true);
        }

        public override Command Copy()
        {
            var x = new UnzipFile(_workFolder, _package);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
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
