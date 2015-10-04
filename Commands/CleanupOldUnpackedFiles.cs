﻿using IwAutoUpdater.CrossCutting.Base;
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
    public class CleanupOldUnpackedFiles : Command
    {
        private readonly IDirectory _directory;
        private readonly string _fullPathToLocalDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public CleanupOldUnpackedFiles(string workFolder, IUpdatePackage package, IDirectory directory)
        {
            _workFolder = workFolder;
            _package = package;
            _directory = directory;

            _fullPathToLocalDirectory = Path.GetFileNameWithoutExtension(Path.Combine(_workFolder, package.Access.GetFilenameOnly()));
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
            _directory.Delete(_fullPathToLocalDirectory);

            return new CommandResult(true);
        }
    }
}