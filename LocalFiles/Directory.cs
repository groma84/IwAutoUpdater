using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.Collections.Generic;

namespace IwAutoUpdater.DAL.LocalFiles
{
    public class Directory : IDirectory
    {
        private readonly ILogger _logger;

        public Directory(ILogger logger)
        {
            _logger = logger;
        }

        void IDirectory.Delete(string fullPath)
        {
            if (System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.Delete(fullPath, true);
                _logger.Debug("Deleted {Foldername}", fullPath);
            }
            else
            {
                _logger.Debug("Path {Foldername} does not exist", fullPath);
            }
        }

        IEnumerable<string> IDirectory.GetFiles(string fullPath, string searchPattern)
        {
            return System.IO.Directory.EnumerateFiles(fullPath, searchPattern);
        }
    }
}
