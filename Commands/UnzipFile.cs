using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;

namespace IwAutoUpdater.BLL.Commands
{
    public class UnzipFile : Command
    {
        private readonly ILogger _logger;
        private readonly IDirectory _directory;
        private readonly string _password;
        private readonly string _fullPathToLocalFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public UnzipFile(string workFolder, string password, IUpdatePackage package, IDirectory directory, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());
            _password = password;
            _directory = directory;
            _logger = logger;
        }

        public override CommandResult Do()
        {
            // das lokale Verzeichnis reicht, weil in der zip-Datei sowieso immer noch 
            // der passende Unterordner drin steckt
            var extractTo = Path.GetDirectoryName(_fullPathToLocalFile);

            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(_fullPathToLocalFile);
                zf = new ZipFile(fs);

                // von https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#unpack-a-zip-with-full-control-over-the-operation
                if (!string.IsNullOrWhiteSpace(_password))
                {
                    zf.Password = _password;
                }

                _directory.CreateIfNotExists(extractTo);

                foreach (ZipEntry entry in zf)
                {
                    if (!entry.IsFile)
                    {
                        continue; // Ignore directories
                    }

                    var entryFileName = entry.Name;

                    var buffer = new byte[4096];
                    var zipStream = zf.GetInputStream(entry);

                    var fullZipToPath = Path.Combine(extractTo, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (directoryName.Length > 0)
                    {
                        _directory.CreateIfNotExists(directoryName);
                    }

                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }

            return new CommandResult(true);
        }

        public override Command Copy()
        {
            var x = new UnzipFile(_workFolder, _password, _package, _directory, _logger);
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
